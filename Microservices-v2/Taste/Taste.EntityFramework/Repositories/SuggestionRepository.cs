using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Muuvis.EntityFramework.Repository;
using Muuvis.Taste.EntityFramework.DataModel;
using Suggestion = Muuvis.Taste.DomainModel.Suggestion;

namespace Muuvis.Taste.EntityFramework.Repositories
{
    internal class SuggestionRepository : DbMapRepository<Suggestion, DataModel.Suggestion, TasteContext>
    {
        private readonly ILogger<SuggestionRepository> _logger;

        public SuggestionRepository(TasteContext context, ILogger<SuggestionRepository> logger) : base(context)
        {
            _logger = logger;
        }

        protected override Task<DataModel.Suggestion> LoadDataModelAsync(string id)
        {
            return Context.Suggestions.FindAsync(id);
        }

        protected override void ToDataModel(Suggestion entity,
            DataModel.Suggestion dataModel)
        {
            dataModel.Id = entity.Id;
            dataModel.MovieId = entity.MovieId;
            dataModel.Affinity = entity.Affinity;
        }

        protected override Suggestion ToDomainModel(DataModel.Suggestion dataModel)
        {
            var result = new Suggestion(dataModel.Id, dataModel.MovieId)
            {
                Affinity = dataModel.Affinity
            };

            return result;
        }
    }
}