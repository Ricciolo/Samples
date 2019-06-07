using Industria4.Cqrs.Messaging.Commands;

namespace Industria4.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents an added event for a specific command related to an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityAddedEvent<T> : EntityCompletedEvent<T>, IEntityAddedEvent
        where T : IEntityType
    {
        public EntityAddedEvent(string id) : base(id)
        {
        }
    }

    /// <summary>
    ///     Represents an added event related to an entity
    /// </summary>
    public interface IEntityAddedEvent : IEntityCompletedEvent
    {
    }
}