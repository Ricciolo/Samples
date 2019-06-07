namespace Industria4.Cqrs.Messaging.Commands
{
    /// <summary>
    ///     Represents a base implementation of a command which removes an entity
    /// </summary>
    public abstract class RemoveEntityCommand<T> : EntityCommand<T>
        where T : IEntityType
    {
        protected RemoveEntityCommand(string id) : base(id)
        {
        }
    }
}