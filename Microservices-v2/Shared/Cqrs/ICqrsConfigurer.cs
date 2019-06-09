using Muuvis.Cqrs.Messaging.Commands;
using Muuvis.Cqrs.Messaging.Events;
using Rebus.Handlers;

namespace Muuvis.Cqrs
{
    /// <summary>
    ///     Interfaces used for configuring service bus
    /// </summary>
    public interface ICqrsConfigurer
    {
        /// <summary>
        ///     Registers all commands availables in the same assembly of type T and set the route to the queue specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ICqrsConfigurer AddCommandsRouteFromAssemblyOfType<T>(string queueName)
            where T : ICommand;

        /// <summary>
        ///     Configures the bus to use an inmemory queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ICqrsConfigurer UseInMemoryQueue(string queueName);

        /// <summary>
        ///     Configures the bus to use RabbitMQ queue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ICqrsConfigurer UseRabbitQueue(string queueName);

        /// <summary>
        ///     Configures the bus to use Azure Service Bus
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ICqrsConfigurer UseAzureServiceBus(string queueName);

        /// <summary>
        ///     Registers all handlers availables in the same assembly of type T for receiving messages
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ICqrsConfigurer AddHandlersFromAssemblyOfType<T>()
            where T : IHandleMessages;

        /// <summary>
        ///     Subscribes the bus to a specific event type
        /// </summary>
        /// <remarks>Don't use an interface because you cannot use polymorphic subscription but only polymorphic handlers</remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ICqrsConfigurer Subscribe<T>()
            where T : IEvent;
    }
}