using Muuvis.Cqrs;

namespace Muuvis.Catalog.Cqrs.Commands
{
    public class AddMovieCommand : CommandBase
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public string Id { get; set; }
    }
}
