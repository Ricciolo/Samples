using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Muuvis.Catalog.WebApi.Models.Movie
{
    public class GetApiModel
    {
        [Key]
        public string Id { get; set; }

        public int Year { get; set; }

        public string Culture { get; set; }

        public string Title { get; set; }

    }
}
