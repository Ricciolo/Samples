namespace Industria4.Cqrs.Messaging.Commands
{
    /// <summary>
    ///     Represents a base implementation of a command which adds an entity
    /// </summary>
    public abstract class AddEntityCommand<T> : EntityCommand<T>
        where T : IEntityType
    {
        protected AddEntityCommand(string id) : base(id)
        {
        }
    }
}