using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.Cqrs;

namespace Muuvis.Taste.Cqrs.Events
{
    public class SuggestionEvaluatedEvent : EventBase
    {
        public string MovieId { get; set; }
    }
}
