using System;
using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents an event for a specific command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandEvent<T> : EventBase, ICommandEvent
        where T : ICommand
    {
        protected CommandEvent(string commandId, object state)
        {
            CommandId = commandId ?? throw new ArgumentNullException(nameof(commandId));
            State = state;
        }

        /// <summary>
        ///     The original command id which has raised the event
        /// </summary>
        public string CommandId { get; }

        /// <summary>
        ///     Gets a generic state information
        /// </summary>
        public object State { get; }

        Type ICommandEvent.CommandType => typeof(T);
    }
}