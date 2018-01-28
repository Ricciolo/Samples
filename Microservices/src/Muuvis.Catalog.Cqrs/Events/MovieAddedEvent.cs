using Muuvis.Cqrs;

namespace Muuvis.Catalog.Cqrs.Events
{
    public class MovieAddedEvent : EventBase
    {
        public string Id { get; set; }
    }
}
