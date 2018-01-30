using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muuvis.Catalog.Api.Models.Movie
{
    public class GetModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }
    }
}
