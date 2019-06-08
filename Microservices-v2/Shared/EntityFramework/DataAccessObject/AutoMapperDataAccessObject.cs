using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Muuvis.EntityFramework.DataAccessObject
{
    /// <summary>
    ///     Base object for querying model using <see cref="DbContext" />. The implementation support a data model and uses
    ///     AutoMapper for mapping data
    /// </summary>
    /// <typeparam name="TDataModel"></typeparam>
    /// <typeparam name="TReadModel"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public class AutoMapperDataAccessObject<TDataModel, TReadModel, TDbContext> : DataAccessObject<TReadModel, TDbContext>
        where TDataModel : class
        where TReadModel : class
        where TDbContext : DbContext
    {
        public AutoMapperDataAccessObject(TDbContext context, IMapper mapper) : base(context)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected override IQueryable<TReadModel> Query => Set.ProjectTo<TReadModel>(Mapper.ConfigurationProvider);

        protected virtual IQueryable<TDataModel> Set => Context.Set<TDataModel>();

        public IMapper Mapper { get; }

    }
}