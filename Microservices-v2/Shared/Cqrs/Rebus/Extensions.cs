using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Muuvis.Cqrs.Messaging.Commands;
using Muuvis.Cqrs.Messaging.Events;
using Muuvis.Cqrs.Rebus;
using Muuvis.DomainModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Exceptions;
using Rebus.Logging;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Rebus.Pipeline.Send;
using Rebus.Retry;
using Rebus.Retry.FailFast;
using Rebus.Retry.Simple;
using Rebus.Serialization;
using Rebus.Transport;

// ReSharper disable once CheckNamespace
namespace Rebus.Bus
{
    /// <summary>
    ///     Extension methods for Rebus
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Publish a CommandErrorEvent
        /// </summary>
        /// <returns></returns>
        public static Task PublishCommandErrorEvent<T>(this IBus bus, T command, Exception exception)
            where T : ICommand
        {
            return PublishCommandErrorEvent(bus, typeof(T), command, exception);
        }

        /// <summary>
        /// Publish a CommandErrorEvent
        /// </summary>
        /// <returns></returns>
        public static async Task PublishCommandErrorEvent(this IBus bus, Type commandType, ICommand command, Exception exception)
        {
            Exception[] exceptions = (exception as AggregateException)?.InnerExceptions.ToArray() ?? new[] { exception };
            // Filter any rebus exceptions (causes a stack overflow)
            exceptions = exceptions.Where(e => !(e is RebusApplicationException)).ToArray();
            if (exceptions.Length == 0) return;

            object eventMessage = Activator.CreateInstance(typeof(CommandErrorEvent<>).MakeGenericType(commandType), command.MessageId, null, exceptions);

            try
            {
                await bus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                // TODO: better error handling. Some exceptions cannot be serialized
                exceptions = exceptions.Select(e => new Exception(e.Message)).ToArray();
                eventMessage = Activator.CreateInstance(typeof(CommandErrorEvent<>).MakeGenericType(commandType), command.MessageId, null, exceptions);
                await bus.Publish(eventMessage);
            }
        }

        /// <summary>
        /// Publish a CommandValidationEvent
        /// </summary>
        /// <returns></returns>
        public static Task PublishCommandValidationEvent<T>(this IBus bus, T command, IEnumerable<ValidationResult> results)
            where T : ICommand
        {
            return PublishCommandValidationEvent(bus, typeof(T), command, results);
        }

        /// <summary>
        /// Publish a CommandValidationEvent
        /// </summary>
        /// <returns></returns>
        public static async Task PublishCommandValidationEvent(this IBus bus, Type commandType, ICommand command, IEnumerable<ValidationResult> results)
        {
            object eventMessage = Activator.CreateInstance(typeof(CommandValidationEvent<>).MakeGenericType(commandType), command.MessageId, null, results);

            await bus.Publish(eventMessage);
        }
    }
}

// ReSharper disable once CheckNamespace
namespace Rebus.Pipeline
{
    /// <summary>
    ///     Extension methods for Rebus
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets when the message has been sent
        /// </summary>
        /// <param name="messageContext"></param>
        /// <returns></returns>
        public static DateTimeOffset GetSentDateTime(this IMessageContext messageContext)
        {
            return DateTimeOffset.Parse(messageContext.Headers[Headers.SentTime]);
        }
    }
}

// ReSharper disable once CheckNamespace
namespace Rebus.Transport
{
    /// <summary>
    ///     Extension methods for Rebus
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Gets the DI scope available inside the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceProvider GetProvider(this ITransactionContext context)
        {
            return context.GetOrNull<IServiceProvider>(nameof(IServiceProvider)) ?? throw new InvalidOperationException("Cannot find the dependency injection scope");
        }
    }
}

// ReSharper disable once CheckNamespace
namespace Rebus.Config
{
    /// <summary>
    ///     Extension methods for Rebus
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        ///     Registers a custom IErrorHandler which emits events for any exceptions raised by a command
        /// </summary>
        /// <param name="configurer"></param>
        /// <param name="provider"></param>
        public static void HandleCommandsEvents(this OptionsConfigurer configurer, IServiceProvider provider)
        {
            configurer.Register<IErrorHandler>(c =>
            {
                var simpleRetryStrategySettings = c.Get<SimpleRetryStrategySettings>();
                var transport = c.Get<ITransport>();
                var serializer = c.Get<ISerializer>();
                var rebusLoggerFactory = c.Get<IRebusLoggerFactory>();
                return new CommandsErrorHandler(provider, serializer, simpleRetryStrategySettings, transport, rebusLoggerFactory);
            });

            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();
                var step = new CompleteCommandEventStep(c.Get<IErrorTracker>(), provider);

                return new PipelineStepInjector(pipeline)
                    .OnReceive(step, PipelineRelativePosition.After, typeof(DispatchIncomingMessageStep));
            });
        }

        /// <summary>
        /// Registers a step which set and load the message id header from IMessage.MessageId
        /// </summary>
        /// <param name="configurer"></param>
        public static void AutoSetMessageId(this OptionsConfigurer configurer)
        {
            var step = new AutoSetMessageIdStep();
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepInjector(pipeline)
                    .OnSend(step, PipelineRelativePosition.Before, typeof(AssignDefaultHeadersStep));
            });
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepInjector(pipeline)
                    .OnReceive(step, PipelineRelativePosition.After, typeof(DeserializeIncomingMessageStep));
            });
        }

        /// <summary>
        /// Registers a step which set and load the message id header from IMessage.MessageId
        /// </summary>
        /// <param name="configurer"></param>
        public static void CatchMessagesSent(this OptionsConfigurer configurer)
        {
            var step = new CatchMessagesSentStep();
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepInjector(pipeline)
                    .OnSend(step, PipelineRelativePosition.After, typeof(SendOutgoingMessageStep));
            });
        }

        /// <summary>
        /// Registers a step which the scope of each pipeline
        /// </summary>
        /// <param name="configurer"></param>
        /// <param name="provider"></param>
        public static void ApplyServiceProvider(this OptionsConfigurer configurer, IServiceProvider provider)
        {
            var step = new ServiceProviderStep(provider);
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();
                var retryStep = c.Get<IRetryStrategyStep>();

                return new PipelineStepInjector(pipeline)
                    .OnReceive(step, PipelineRelativePosition.Before, retryStep.GetType());
            });
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepInjector(pipeline)
                    .OnSend(step, PipelineRelativePosition.Before, typeof(AssignDefaultHeadersStep));
            });
        }
    }
}