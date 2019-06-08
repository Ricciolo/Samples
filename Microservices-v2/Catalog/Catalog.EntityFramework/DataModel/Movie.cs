using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Muuvis.Catalog.EntityFramework.DataModel
{
	internal class Movie
	{
        [StringLength(36)]
		public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string OriginalTitle { get; set; }

        [Required]
        [StringLength(6)]
        public CultureInfo OriginalCulture { get; set; }

        public int Year { get; set; }

        public bool IsDeleted { get; set; }

        public IList<TitleTranslation> Translations { get; set; } = new List<TitleTranslation>();
    }
}
