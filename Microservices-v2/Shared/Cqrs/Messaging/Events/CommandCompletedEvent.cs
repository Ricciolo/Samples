﻿using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents an completed event for a specific command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandCompletedEvent<T> : CommandEvent<T>
        where T : ICommand
    {
        public CommandCompletedEvent(string commandId, object state) : base(commandId, state)
        {
        }
    }
}