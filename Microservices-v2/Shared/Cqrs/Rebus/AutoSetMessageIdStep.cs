using System;
using System.Threading.Tasks;
using Industria4.Cqrs.Messaging;
using Rebus.Bus;
using Rebus.Messages;
using Rebus.Pipeline;

namespace Industria4.Cqrs.Rebus
{
    /// <summary>
    /// Automatically set message id and correlation headers
    /// </summary>
    public class AutoSetMessageIdStep : IOutgoingStep, IIncomingStep
    {
        public Task Process(OutgoingStepContext context, Func<Task> next)
        {
            var message = context.Load<Message>();
            if (message.Body is IMessage m)
            {
                message.Headers[Headers.MessageId] = m.MessageId;
            }

            return next();
        }

        public Task Process(IncomingStepContext context, Func<Task> next)
        {
            var message = context.Load<Message>();
            if (message.Body is IMessage m)
            {
                m.MessageId = message.GetMessageId();
            }

            return next();
        }
    }
}
