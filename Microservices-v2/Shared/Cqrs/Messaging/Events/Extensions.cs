using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Industria4.Cqrs.Messaging.Commands;
using Industria4.DomainModel;

namespace Industria4.Cqrs.Messaging.Events
{
    /// <summary>
    /// Extensions methods for creating events
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates a <see cref="EntityAddedEvent"/> for a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static EntityAddedEvent<T> CreateAddedEvent<T>(this EntityCommand<T> command)
            where T : IEntityType
        {
            return new EntityAddedEvent<T>(command.Id);
        }

        /// <summary>
        /// Creates a <see cref="EntityRemovedEvent"/> for a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static EntityRemovedEvent<T> CreateRemovedEvent<T>(this EntityCommand<T> command)
            where T : IEntityType
        {
            return new EntityRemovedEvent<T>(command.Id);
        }

        /// <summary>
        /// Creates a <see cref="EntityUpdatedEvent"/> for a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static EntityUpdatedEvent<T> CreateUpdatedEvent<T>(this EntityCommand<T> command)
            where T : IEntityType
        {
            return new EntityUpdatedEvent<T>(command.Id);
        }

        /// <summary>
        /// Creates a <see cref="CommandCompletedEvent"/> for a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public static CommandCompletedEvent<T> CreateCompletedEvent<T>(this T command)
            where T : ICommand
        {
            return new CommandCompletedEvent<T>(command.MessageId, null);
        }

        /// <summary>
        /// Creates a <see cref="CommandValidationEvent"/> for a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="state"></param>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        public static CommandValidationEvent<T> CreateValidationEvent<T>(this T command, object state, params ValidationResult[] validationResults)
            where T : ICommand
        {
            return new CommandValidationEvent<T>(command.MessageId, state, validationResults);
        }

        /// <summary>
        /// Creates a <see cref="CommandValidationEvent"/> for a command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        public static CommandValidationEvent<T> CreateValidationEvent<T>(this T command, params ValidationResult[] validationResults)
            where T : ICommand
        {
            return new CommandValidationEvent<T>(command.MessageId, null, validationResults);
        }

    }
}
