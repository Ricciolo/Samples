using Industria4.Cqrs.Messaging.Commands;
using Industria4.Cqrs.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Industria4.Cqrs
{
    public static class EventsConfigurerExtensions
    {
        /// <summary>
        ///     Subscribes the bus to all related events for a command
        /// </summary>
        /// <remarks>Don't use an interface because you cannot use polymorphic subscription but only polymorphic handlers</remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICqrsConfigurer SubscribeCommandEvents<T>(this ICqrsConfigurer configurer)
            where T : ICommand
        {
            configurer.Subscribe<CommandCompletedEvent<T>>();
            configurer.Subscribe<CommandErrorEvent<T>>();
            configurer.Subscribe<CommandValidationEvent<T>>();

            return configurer;
        }
    }
}
