using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Taste.Cqrs.Commands.Suggestion
{
    public class EvaluateSuggestionCommand : CommandBase
    {
        public EvaluateSuggestionCommand(string movieId)
        {
            MovieId = movieId;
        }

        public string MovieId { get; }
    }
}