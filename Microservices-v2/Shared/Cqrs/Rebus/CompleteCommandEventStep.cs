using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Industria4.Cqrs.Messaging;
using Industria4.Cqrs.Messaging.Commands;
using Industria4.Cqrs.Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Retry;
using Rebus.Transport;

namespace Industria4.Cqrs.Rebus
{
    public class CompleteCommandEventStep : IIncomingStep
    {
        private readonly IErrorTracker _errorTracker;
        private readonly IServiceProvider _serviceProvider;

        public CompleteCommandEventStep(IErrorTracker errorTracker, IServiceProvider serviceProvider)
        {
            _errorTracker = errorTracker;
            _serviceProvider = serviceProvider;
        }

        public async Task Process(IncomingStepContext context, Func<Task> next)
        {
            var message = context.Load<Message>();
            string messageId = message.GetMessageId();
            if (message.Body is ICommand command && !_errorTracker.HasFailedTooManyTimes(messageId))
            {
                var currentTransactionContext = context.Load<ITransactionContext>();
                var messagesCatcher = currentTransactionContext.GetProvider().GetService<IMessagesCatcher>();
                if (!currentTransactionContext.GetIsAutoCompleteEventDisabled() 
                    && (messagesCatcher == null || !messagesCatcher.Messages.OfType<ICommandEvent>().Any()))
                {
                    object eventMessage = CreateEvent(command);
                    var bus = _serviceProvider.GetRequiredService<IBus>();
                    await bus.Publish(eventMessage);
                }
            }
            await next();
        }

        private object CreateEvent(ICommand command)
        {
            return Activator.CreateInstance(typeof(CommandCompletedEvent<>).MakeGenericType(command.GetType()), command.MessageId, null);
        }
    }
}
