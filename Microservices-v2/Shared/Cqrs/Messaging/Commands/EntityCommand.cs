using System;

namespace Muuvis.Cqrs.Messaging.Commands
{
    /// <summary>
    ///     Represents a base implementation of a command which works with an entity
    /// </summary>
    public abstract class EntityCommand<T> : CommandBase, IEntityCommand
        where T : IEntityType
    {
        protected EntityCommand(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        ///     Gets the entity id
        /// </summary>
        public string Id { get; }
    }
}