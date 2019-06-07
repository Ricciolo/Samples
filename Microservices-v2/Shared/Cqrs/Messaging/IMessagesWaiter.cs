using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Industria4.Cqrs.Messaging
{
    public interface IMessagesWaiter
    {
        Task<IMessageWaiter> GetAsync(IEnumerable<Type> messageTypes, Func<IMessage, bool> filter);
    }
}