using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Muuvis.Cqrs.Messaging
{
    public interface IMessageWaiter : IDisposable
    {
        Task<IMessage> WhenAsync(TimeSpan timeout);      
    }
}
