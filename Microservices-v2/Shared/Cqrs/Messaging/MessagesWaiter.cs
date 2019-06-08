using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Handlers;

namespace Muuvis.Cqrs.Messaging
{
    internal class MessagesWaiter : MessagesWaiterBase, IHandleMessages<IMessage>
    {
        private readonly IBus _bus;

        public MessagesWaiter(IBus bus)
        {
            _bus = bus;
        }

        Task IHandleMessages<IMessage>.Handle(IMessage message)
        {
            Handle(message);

            return Task.CompletedTask;
        }

        public override async Task<IMessageWaiter> GetAsync(IEnumerable<Type> messageTypes, Func<IMessage, bool> filter)
        {
            IMessageWaiter messageWaiter = await base.GetAsync(messageTypes, filter);

            foreach (Type messageType in messageTypes)
            {
                await _bus.Subscribe(messageType);
            }

            return messageWaiter;
        }
    }
}