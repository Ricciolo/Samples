using System.ComponentModel.DataAnnotations;

namespace Muuvis.Taste.WebApi.Models.Suggestion
{
    public class GetApiModel
    {
        [Key]
        public string Id { get; set; }

        public string MovieId { get; set; }

        public float Affinity { get; set; }

    }
}
