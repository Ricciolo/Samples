using Industria4.Cqrs.Messaging.Commands;

namespace Industria4.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents an updated event for a specific command related to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdatedEvent<T> : EntityCompletedEvent<T>, IEntityUpdatedEvent
        where T : IEntityType
    {
        public EntityUpdatedEvent(string id) : base(id)
        {
        }
    }

    /// <summary>
    ///     Represents an updated event related to an entity
    /// </summary>
    public interface IEntityUpdatedEvent : IEntityCompletedEvent
    {
    }
}