using System;
using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents a completetion event for a specific command related to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityCompletedEvent<T> : EventBase, IEntityCompletedEvent
        where T : IEntityType
    {
        protected EntityCompletedEvent(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        /// Gets the entity id
        /// </summary>
        public string Id { get; }
    }

    /// <summary>
    ///     Represents an completetion event related to an entity
    /// </summary>
    public interface IEntityCompletedEvent : IEntityEvent
    {
    }
}