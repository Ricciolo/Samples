using System;
using System.Threading.Tasks;
using Industria4.Cqrs.Messaging.Commands;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Logging;
using Rebus.Messages;
using Rebus.Retry;
using Rebus.Retry.PoisonQueues;
using Rebus.Retry.Simple;
using Rebus.Serialization;
using Rebus.Transport;

namespace Industria4.Cqrs.Rebus
{
    internal class CommandsErrorHandler : IErrorHandler, IInitializable
    {
        private readonly PoisonQueueErrorHandler _innerHandler;
        private readonly ISerializer _serializer;
        private readonly IServiceProvider _serviceProvider;

        public CommandsErrorHandler(IServiceProvider serviceProvider, ISerializer serializer, SimpleRetryStrategySettings simpleRetryStrategySettings, ITransport transport, IRebusLoggerFactory rebusLoggerFactory)
        {
            _serviceProvider = serviceProvider;
            _serializer = serializer;
            _innerHandler = new PoisonQueueErrorHandler(simpleRetryStrategySettings, transport, rebusLoggerFactory);
        }

        public async Task HandlePoisonMessage(TransportMessage transportMessage, ITransactionContext transactionContext, Exception exception)
        {
            await _innerHandler.HandlePoisonMessage(transportMessage, transactionContext, exception);

            // Verity if the message is a command
            Type messageType = Type.GetType(transportMessage.GetMessageType(), true);
            if (typeof(ICommand).IsAssignableFrom(messageType))
            {
                var bus = _serviceProvider.GetRequiredService<IBus>();

                // Deserialize the message to get original command
                Message message = await _serializer.Deserialize(transportMessage);
                if (message.Body is ICommand command)
                {
                    // Realign message id
                    command.MessageId = message.GetMessageId();
                    await bus.PublishCommandErrorEvent(messageType, command, exception);
                }
            }
        }

        public void Initialize()
        {
            _innerHandler.Initialize();
        }

    }
}