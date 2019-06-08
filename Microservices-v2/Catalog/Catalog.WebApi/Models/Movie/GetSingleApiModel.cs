using System;
using System.Collections.Generic;
using System.Linq;

namespace Muuvis.Catalog.WebApi.Models.Movie
{
    public class GetSingleApiMovie : Movie
    {
        public GetSingleApiMovie(DomainModel.Movie movie)
        {
            Id = movie.Id;
            OriginalCulture = movie.Translation.OriginalCulture;
            OriginalTitle = movie.OriginalTitle;
            Year = movie.Year;
            Translation = movie.Translation.ToDictionary(t => t.Key, t => t.Value);
        }

        public string Id { get; }

    }

}
