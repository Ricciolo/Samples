using System.Linq;
using Muuvis.EntityFramework.DataAccessObject;

namespace Muuvis.Catalog.EntityFramework.DataAccessObject
{
    internal class CatalogDataAccessObject<TDataModel, TReadModel> : AutoMapperDataAccessObject<TDataModel, TReadModel, DataModel.CatalogContext>
        where TDataModel : class
        where TReadModel : class
    {
        public CatalogDataAccessObject(DataModel.CatalogContext context, CatalogMapper mapperAccessor) : base(context, mapperAccessor.Mapper)
        {

        }

    }
}
