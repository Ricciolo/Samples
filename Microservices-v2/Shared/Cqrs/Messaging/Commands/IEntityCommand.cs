namespace Industria4.Cqrs.Messaging.Commands
{
    /// <summary>
    ///     Represents a command dedicated to an entity
    /// </summary>
    public interface IEntityCommand : ICommand
    {
        /// <summary>
        ///     Gets the entity id
        /// </summary>
        string Id { get; }
    }
}