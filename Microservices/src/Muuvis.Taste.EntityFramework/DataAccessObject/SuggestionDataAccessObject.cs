using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Muuvis.Taste.EntityFramework.DataModel;
using Muuvis.Taste.ReadModel;
using Muuvis.EntityFramework.DataAccessObject;

namespace Muuvis.Taste.EntityFramework.DataAccessObject
{
    internal class SuggestionDataAccessObject : DataAccessObject<DomainModel.Suggestion, ReadModel.Suggestion, TasteEntities>
    {
        public SuggestionDataAccessObject(TasteEntities context) : base(context)
        {
        }

        protected override IQueryable<Suggestion> Query => Context.Suggestions.Select(m => new Suggestion
        {
            Id = m.Id,
            MovieId = m.MovieId,
        });
    }
}
