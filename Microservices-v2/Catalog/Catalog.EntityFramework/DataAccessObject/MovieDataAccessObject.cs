using System.Linq;
using Muuvis.Catalog.EntityFramework.DataModel;

namespace Muuvis.Catalog.EntityFramework.DataAccessObject
{
    internal class MovieDataAccessObject : CatalogDataAccessObject<DataModel.TitleTranslation, ReadModel.MovieRead>
    {
        public MovieDataAccessObject(CatalogContext context, CatalogMapper mapperAccessor) : base(context, mapperAccessor)
        {
        }

        protected override IQueryable<DataModel.TitleTranslation> Set => base.Set.Where(m => !m.Movie.IsDeleted);
    }
}
