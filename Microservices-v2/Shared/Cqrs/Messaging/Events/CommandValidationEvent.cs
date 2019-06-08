using System;
using System.Collections.Generic;
using Muuvis.Cqrs.Messaging.Commands;
using Muuvis.DomainModel;

namespace Muuvis.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents a validation event for a specific command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandValidationEvent<T> : CommandEvent<T>, ICommandValidationEvent
        where T : ICommand
    {

        public CommandValidationEvent(string commandId, object state, IEnumerable<ValidationResult> results) : base(commandId, state)
        {
            Results = results ?? throw new ArgumentNullException(nameof(results));
        }

        /// <summary>
        ///     Gets the results
        /// </summary>
        public IEnumerable<ValidationResult> Results { get; }
    }

    /// <summary>
    /// Represents an error event for a specific command
    /// </summary>
    public interface ICommandValidationEvent : ICommandEvent
    {
        /// <summary>
        ///     Gets the results
        /// </summary>
        IEnumerable<ValidationResult> Results { get; }
    }
}