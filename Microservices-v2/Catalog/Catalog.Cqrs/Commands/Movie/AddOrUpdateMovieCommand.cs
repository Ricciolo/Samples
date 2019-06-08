using System;
using System.Collections.Generic;
using System.Globalization;
using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Catalog.Cqrs.Commands.Movie
{
    public class AddOrUpdateMovieCommand : AddOrUpdateEntityCommand<MovieType>
    {
        public AddOrUpdateMovieCommand(string id,
                                       string originalTitle,
                                       CultureInfo originalCulture,
                                       int year) : base(id)
        {
            OriginalTitle = originalTitle ?? throw new ArgumentNullException(nameof(originalTitle));
            OriginalCulture = originalCulture ?? throw new ArgumentNullException(nameof(originalCulture));
            Year = year;
        }

        public string OriginalTitle { get; }

        public CultureInfo OriginalCulture { get; }

        public int Year { get; }

        public Dictionary<CultureInfo, string> Translation { get; } = new Dictionary<CultureInfo, string>();
    }
}
