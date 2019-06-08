using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Cqrs.Messaging.Events
{
    /// <summary>
    /// Represents an error event for a specific command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandErrorEvent<T> : CommandEvent<T>, ICommandErrorEvent
        where T : ICommand
    {

        /// <summary>
        /// Gets the exceptions raised by the command
        /// </summary>
        public Exception[] Exceptions { get; }

        public CommandErrorEvent(string commandId, object state, Exception[] exceptions) : base(commandId, state)
        {
            Exceptions = exceptions ?? throw new ArgumentNullException(nameof(exceptions));
        }
    }

    /// <summary>
    /// Represents an error event for a specific command
    /// </summary>
    public interface ICommandErrorEvent : ICommandEvent
    {
        /// <summary>
        /// Gets the exceptions raised by the command
        /// </summary>
        Exception[] Exceptions { get; }
    }
}
