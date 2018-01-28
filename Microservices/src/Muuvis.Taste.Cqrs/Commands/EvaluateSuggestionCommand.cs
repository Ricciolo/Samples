using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.Cqrs;

namespace Muuvis.Taste.Cqrs.Commands
{
    public class EvaluateSuggestionCommand : CommandBase
    {
        public string MovieId { get; set; }

    }
}
