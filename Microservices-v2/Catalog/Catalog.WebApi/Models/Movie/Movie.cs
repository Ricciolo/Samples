using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Muuvis.Catalog.Cqrs.Commands.Movie;
using Muuvis.Catalog.DomainModel;

namespace Muuvis.Catalog.WebApi.Models.Movie
{
	public class Movie
	{
        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(255)]
        public string OriginalTitle { get; set; }

        [StringLength(6)]
        public CultureInfo OriginalCulture { get; set; }

        public Dictionary<CultureInfo, string> Translation { get; set; }

        public AddOrUpdateMovieCommand GetCommand(string id)
        {
            var command = new AddOrUpdateMovieCommand(id, OriginalTitle, OriginalCulture ?? TitleTranslation.Default, Year);
            if (Translation != null)
            {
                foreach ((CultureInfo key, string value) in Translation)
                {
                    command.Translation.Add(key, value);
                }
            }

            return command;
        }
    }
}