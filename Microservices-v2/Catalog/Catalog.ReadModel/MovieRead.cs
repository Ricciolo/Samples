using System.Collections.Generic;
using System.Globalization;

namespace Muuvis.Catalog.ReadModel
{
	public class MovieRead
    {
		public string Id { get; set; }

        public string Title { get; set; }

        public CultureInfo Culture { get; set; }

        public int Year { get; set; }

        public bool IsOriginal { get; set; }
    }
}
