using Muuvis.EntityFramework.DataAccessObject;
using Muuvis.Taste.EntityFramework.DataModel;

namespace Muuvis.Taste.EntityFramework.DataAccessObject
{
    internal class TasteDataAccessObject<TDataModel, TReadModel> : AutoMapperDataAccessObject<TDataModel, TReadModel, TasteContext>
        where TDataModel : class
        where TReadModel : class
    {
        public TasteDataAccessObject(TasteContext context, TasteMapper mapperAccessor) : base(context, mapperAccessor.Mapper)
        {

        }

    }
}
