using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Muuvis.Catalog.EntityFramework.DataModel
{
    internal class TitleTranslation
    {
        [Key]
        public string MovieId { get; set; }

        [Key]
        [StringLength(6)]
        public CultureInfo Culture { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }

        public bool IsOriginal { get; set; }
    }
}
