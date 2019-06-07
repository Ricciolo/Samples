using Industria4.Cqrs.Messaging.Commands;

namespace Industria4.Cqrs.Messaging.Events
{
    /// <summary>
    ///     Represents a removed event for a specific command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityRemovedEvent<T> : EntityCompletedEvent<T>, IEntityRemovedEvent
        where T : IEntityType
    {
        public EntityRemovedEvent(string id) : base(id)
        {
        }
    }

    /// <summary>
    ///     Represents a removed event for a specific command
    /// </summary>
    public interface IEntityRemovedEvent : IEntityCompletedEvent
    {
    }
}