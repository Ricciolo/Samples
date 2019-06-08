using Muuvis.DomainModel;

namespace Muuvis.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents a CQRS event dedicated to an <see cref="IEntity" />
    /// </summary>
    public interface IEntityEvent
    {
        /// <summary>
        /// Gets the entity id
        /// </summary>
        string Id { get; }
    }
}