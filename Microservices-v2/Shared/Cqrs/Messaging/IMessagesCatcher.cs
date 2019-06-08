using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Muuvis.Cqrs.Messaging
{
    public interface IMessagesCatcher
    {
        IEnumerable<IMessage> Messages { get; }

        Action<IMessage> OnNewMessage { get; set; }

        void Add(IMessage message);

    }
}
