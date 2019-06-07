namespace Industria4.Cqrs.Messaging
{
    /// <summary>
    /// Represents a bus message
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The message id
        /// </summary>
        string MessageId { get; set; }
    }
}
