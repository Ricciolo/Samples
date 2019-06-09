using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Taste.Cqrs.Commands.Suggestion
{
    public class AddOrUpdateSuggestionCommand : AddOrUpdateEntityCommand<SuggestionType>
    {
        public AddOrUpdateSuggestionCommand(string id,
            string movieId,
            float affinity) : base(id)
        {
            MovieId = movieId;
            Affinity = affinity;
        }

        public string MovieId { get; }

        public float Affinity { get; }
    }
}