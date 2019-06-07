using System;

namespace Industria4.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents a CQRS event related to a command
    /// </summary>
    public interface ICommandEvent : IEvent
    {
        /// <summary>
        ///     Gets the command type for this event
        /// </summary>
        Type CommandType { get; }

        /// <summary>
        ///     Gets a generic state information
        /// </summary>
        object State { get; }

        /// <summary>
        ///     The original command id which has raised the event
        /// </summary>
        string CommandId { get; }
    }
}