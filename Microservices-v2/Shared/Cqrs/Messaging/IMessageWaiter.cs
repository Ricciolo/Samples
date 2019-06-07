using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Industria4.Cqrs.Messaging
{
    public interface IMessageWaiter : IDisposable
    {
        Task<IMessage> WhenAsync(TimeSpan timeout);      
    }
}
