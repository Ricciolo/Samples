using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Muuvis.EntityFramework.Repository;

namespace Muuvis.Catalog.EntityFramework.Repositories
{
    internal class MovieRepository : DbMapRepository<DomainModel.Movie, DataModel.Movie, DataModel.CatalogContext>
    {
        private readonly ILogger<MovieRepository> _logger;

        public MovieRepository(DataModel.CatalogContext context, ILogger<MovieRepository> logger) : base(context)
        {
            _logger = logger;
        }

        protected override async Task<DataModel.Movie> LoadDataModelAsync(string id)
        {
            DataModel.Movie result = await Context.Movies.FindAsync(id);
            await Context.Entry(result).Collection(m => m.Translations).LoadAsync();

            return result;
        }

        protected override void ToDataModel(DomainModel.Movie entity,
                                            DataModel.Movie dataModel)
        {
            dataModel.Id = entity.Id;
            dataModel.OriginalTitle = entity.OriginalTitle;
            dataModel.OriginalCulture = entity.Translation.OriginalCulture;
            dataModel.Year = entity.Year;
            dataModel.IsDeleted = entity.IsDeleted;
            Merge(
                entity.Translation,
                dataModel.Translations,
                (e, d) => Equals(e.Key, d.Culture),
               (e, d) => ToDataModel(e, d, entity.Translation.OriginalCulture));
        }

        private void ToDataModel(KeyValuePair<CultureInfo, string> entity, DataModel.TitleTranslation dataModel, CultureInfo originalCulture)
        {
            dataModel.Culture = entity.Key;
            dataModel.Title = entity.Value;
            dataModel.IsOriginal = entity.Key.Equals(originalCulture);
        }

        protected override DomainModel.Movie ToDomainModel(DataModel.Movie dataModel)
        {
            DomainModel.Movie result = new DomainModel.Movie(dataModel.Id, dataModel.OriginalCulture, dataModel.OriginalTitle)
            {
                IsDeleted = dataModel.IsDeleted,
                Year = dataModel.Year,
                Translation = { OriginalCulture = dataModel.OriginalCulture }
            };

            // Populate translation
            foreach (DataModel.TitleTranslation dataModelTranslation in dataModel.Translations)
            {
                result.Translation.Add(dataModelTranslation.Culture, dataModelTranslation.Title);
            }
            return result;
        }

    }
}
