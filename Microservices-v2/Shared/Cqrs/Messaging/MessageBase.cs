using System;
using System.Collections.Generic;
using System.Text;

namespace Industria4.Cqrs.Messaging
{
    public abstract class MessageBase : IMessage
    {
        private string _messageId;

        public string MessageId
        {
            get => _messageId ?? (_messageId = Guid.NewGuid().ToString("D"));
            set => _messageId = value;
        }
    }
}
