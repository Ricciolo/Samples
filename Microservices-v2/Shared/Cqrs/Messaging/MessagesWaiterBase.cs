using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Muuvis.Cqrs.Messaging
{
    public class MessagesWaiterBase : IMessagesWaiter
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<IInternalMessageWaiter, bool>> _messageWaiters = new ConcurrentDictionary<Type, ConcurrentDictionary<IInternalMessageWaiter, bool>>();

        protected virtual void Handle(IMessage message)
        {
            var key = message.GetType();
            if (_messageWaiters.TryGetValue(key, out ConcurrentDictionary<IInternalMessageWaiter, bool> eventWaiter))
            {
                foreach (var pair in eventWaiter)
                {
                    pair.Key.Handle(message);
                }
            }
        }

        protected IEnumerable<Type> PendingTypes => _messageWaiters.Keys;

        protected IEnumerable<IDisposable> GetPending(Type type)
        {
            if (_messageWaiters.TryGetValue(type, out var dic))
            {
                return dic.Keys;
            }

            return Enumerable.Empty<IDisposable>();
        }

        protected virtual void Release(IEnumerable<Type> messageTypes, IMessageWaiter messageWaiter)
        {
            MessageWaiterGroup mwg = (MessageWaiterGroup) messageWaiter;
            foreach (Type messageType in messageTypes)
            {
                if (_messageWaiters.TryGetValue(messageType, out var list))
                {
                    mwg.MessageWaiters.ForEach(m => list.TryRemove(m, out _));
                }
            }
        }

        public virtual Task<IMessageWaiter> GetAsync(IEnumerable<Type> messageTypes, Func<IMessage, bool> filter)
        {
            IEnumerable<Type> messageTypesArray = messageTypes as Type[] ?? messageTypes.ToArray();

            // Create one waiter for message type
            var messageWaitersGroup = new List<MessageWaiter>();
            foreach (Type messageType in messageTypesArray)
            {
                ConcurrentDictionary<IInternalMessageWaiter, bool> eventWaiter = _messageWaiters.GetOrAdd(messageType, o => new ConcurrentDictionary<IInternalMessageWaiter, bool>());

                var messageWaiter = new MessageWaiter(messageType, filter);
                eventWaiter.TryAdd(messageWaiter, true);

                // Keep track of the group
                messageWaitersGroup.Add(messageWaiter);
            }

            return Task.FromResult<IMessageWaiter>(new MessageWaiterGroup(this, messageTypesArray, messageWaitersGroup));
        }

        private class MessageWaiterGroup : IMessageWaiter
        {
            private readonly MessagesWaiterBase _owner;
            private readonly IEnumerable<Type> _messageTypesArray;
            private bool _disposed;

            public MessageWaiterGroup(MessagesWaiterBase owner, IEnumerable<Type> messageTypesArray, List<MessageWaiter> messageWaiters)
            {
                _owner = owner;
                _messageTypesArray = messageTypesArray;
                MessageWaiters = messageWaiters;
            }

            internal List<MessageWaiter> MessageWaiters { get; }

            public void Dispose()
            {
                MessageWaiters.ForEach(d => d.Dispose());
                _disposed = true;
                _owner.Release(_messageTypesArray, this);
            }

            public async Task<IMessage> WhenAsync(TimeSpan timeout)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(IMessageWaiter));

                return await await Task.WhenAny(MessageWaiters.Select(m => m.WhenAsync(timeout)));
            }
        }

        private interface IInternalMessageWaiter : IDisposable
        {
            void Handle(IMessage message);
        }

        private class MessageWaiter : IInternalMessageWaiter
        {
            private readonly List<TaskCompletionSource<IMessage>> _taskCompletionSources = new List<TaskCompletionSource<IMessage>>();
            private readonly CancellationTokenSource _taskCancellation = new CancellationTokenSource();
            private readonly Type _messageType;
            private readonly Func<IMessage, bool> _filter;
            private readonly Queue<IMessage> _backlog = new Queue<IMessage>();
            private bool _closed;

            public MessageWaiter(Type messageType, Func<IMessage, bool> filter)
            {
                _messageType = messageType;
                _filter = filter;
            }

            public Task<IMessage> WhenAsync(TimeSpan timeout)
            {
                lock (this)
                {
                    var taskCompletionSource = new TaskCompletionSource<IMessage>();
                    Timeout(timeout, _taskCancellation.Token).ContinueWith(t =>
                    {
                        _taskCompletionSources.Remove(taskCompletionSource);
                        return taskCompletionSource.TrySetException(new TimeoutException());
                    });

                    _taskCompletionSources.Add(taskCompletionSource);

                    while (_taskCompletionSources.Count > 0 && _backlog.Count > 0)
                    {
                        IMessage message = _backlog.Dequeue();
                        PushMessage(message);
                    }

                    return taskCompletionSource.Task;
                }
            }

            public void Handle(IMessage message)
            {
                if (!_filter(message) || _closed) return;

                PushMessage(message);
            }

            private void PushMessage(IMessage message)
            {
                lock (this)
                {
                    bool sent = false;
                    while (_taskCompletionSources.Count > 0)
                    {
                        TaskCompletionSource<IMessage> tc = _taskCompletionSources[0];
                        _taskCompletionSources.RemoveAt(0);
                        if (tc.TrySetResult(message))
                        {
                            sent = true;
                            break;
                        }
                    }

                    if (!sent)
                    {
                        _backlog.Enqueue(message);
                    }
                }
            }

            public void Dispose()
            {
                _taskCompletionSources.ForEach(tc => tc.TrySetCanceled());
                _taskCancellation.Cancel();
                _backlog.Clear();
                _closed = true;
            }

            private async Task Timeout(TimeSpan timeout, CancellationToken token)
            {
                do
                {
                    Debug.WriteLine($"Waiting {_messageType}");

                    await Task.Delay(timeout, token);

                    if (_taskCompletionSources != null && _taskCompletionSources.Count > 0)
                    {
                        Debug.WriteLine($"No message of {_messageType} received yet");
                    }
                } while (!_closed && false); // Loop used only for debugging purpose //Debugger.IsAttached);
            }
        }
    }
}