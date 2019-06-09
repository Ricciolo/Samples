using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Muuvis.Taste.Cqrs.Commands.Suggestion;

namespace Muuvis.Taste.WebApi.Models.Suggestion
{
	public class Suggestion
	{
        [Required]
        public float Affinity { get; set; }

        [Required]
        [StringLength(36)]
        public string MovieId { get; set; }

        public AddOrUpdateSuggestionCommand GetCommand(string id)
        {
            var command = new AddOrUpdateSuggestionCommand(id, MovieId, Affinity);

            return command;
        }
    }
}