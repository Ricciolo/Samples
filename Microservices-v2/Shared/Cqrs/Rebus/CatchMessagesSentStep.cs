using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Muuvis.Cqrs.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Transport;

namespace Muuvis.Cqrs.Rebus
{
    public class CatchMessagesSentStep : IOutgoingStep
    {
        public Task Process(OutgoingStepContext context, Func<Task> next)
        {
            var currentTransactionContext = context.Load<ITransactionContext>();
            var message = context.Load<Message>();
            var messagesCatcher = currentTransactionContext.GetProvider().GetRequiredService<IMessagesCatcher>();
            if (message.Body is IMessage m)
            {
                messagesCatcher?.Add(m);
            }

            return next();
        }


    }
}
