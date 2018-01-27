using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Muuvis.Catalog.EntityFramework.DataModel;
using Muuvis.Catalog.ReadModel;
using Muuvis.EntityFramework.DataAccessObject;

namespace Muuvis.Catalog.EntityFramework.DataAccessObject
{
    internal class MovieDataAccessObject : DataAccessObject<DomainModel.Movie, ReadModel.Movie, CatalogEntities>
    {
        public MovieDataAccessObject(CatalogEntities context) : base(context)
        {
        }

        protected override IQueryable<Movie> Query => Context.Movies.Select(m => new Movie
        {
            Id = m.Id,
            Title = m.Title,
            Year = m.Year
        });
    }
}
