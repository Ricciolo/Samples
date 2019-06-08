using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rebus.Bus;

namespace Muuvis.Cqrs.Messaging
{
    public class MessagesCatcher : IMessagesCatcher
    {
        private readonly IBus _bus;
        private readonly ConcurrentBag<IMessage> _messages = new ConcurrentBag<IMessage>();

        public MessagesCatcher(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public IEnumerable<IMessage> Messages => _messages;

        public Action<IMessage> OnNewMessage { get; set; }

        public void Add(IMessage message)
        {
            _messages.Add(message);

            OnNewMessage?.Invoke(message);
        }

    }
}
