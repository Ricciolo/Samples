namespace Muuvis.Taste.WebApi.Models.Suggestion
{
    public class GetSingleApiSuggestion : Suggestion
    {
        public GetSingleApiSuggestion(DomainModel.Suggestion suggestion)
        {
            Id = suggestion.Id;
            MovieId = suggestion.MovieId;
            Affinity = suggestion.Affinity;
        }

        public string Id { get; }

    }

}
