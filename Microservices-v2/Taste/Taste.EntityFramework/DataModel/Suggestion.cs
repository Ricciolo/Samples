using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Muuvis.Taste.EntityFramework.DataModel
{
	internal class Suggestion
	{
        [StringLength(36)]
		public string Id { get; set; }

        [Required]
        [StringLength(36)]
        public string MovieId { get; set; }

        public float Affinity { get; set; }
    }
}
