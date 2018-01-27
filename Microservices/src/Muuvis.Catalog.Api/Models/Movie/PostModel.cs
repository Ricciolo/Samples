using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Muuvis.Catalog.Api.Models.Movie
{
    public class PostModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
