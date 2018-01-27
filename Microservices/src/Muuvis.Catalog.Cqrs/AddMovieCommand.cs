using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.Cqrs;

namespace Muuvis.Catalog.Cqrs
{
    public class AddMovieCommand : ICommand
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public string Id { get; set; }
    }
}
