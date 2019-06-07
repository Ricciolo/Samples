namespace Industria4.Cqrs.Messaging.Commands
{
    /// <summary>
    ///     Represents a base implementation of a command which adds or updates an entity
    /// </summary>
    public abstract class AddOrUpdateEntityCommand<T> : EntityCommand<T>
        where T : IEntityType
    {
        protected AddOrUpdateEntityCommand(string id) : base(id)
        {
        }
    }
}