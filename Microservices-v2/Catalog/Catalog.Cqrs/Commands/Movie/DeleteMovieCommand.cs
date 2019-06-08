using Muuvis.Cqrs.Messaging.Commands;

namespace Muuvis.Catalog.Cqrs.Commands.Movie
{
	public class DeleteMovieCommand : RemoveEntityCommand<MovieType>
	{
		public DeleteMovieCommand(string id) : base(id)
		{
		}
	}
}
