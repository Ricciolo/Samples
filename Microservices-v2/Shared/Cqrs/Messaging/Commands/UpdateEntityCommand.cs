namespace Industria4.Cqrs.Messaging.Commands
{
    /// <summary>
    ///     Represents a base implementation of a command which updates an entity
    /// </summary>
    public abstract class UpdateEntityCommand<T> : EntityCommand<T>
        where T : IEntityType
    {
        protected UpdateEntityCommand(string id) : base(id)
        {
        }
    }
}