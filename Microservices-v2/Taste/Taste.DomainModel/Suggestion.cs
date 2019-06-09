using System;
using System.Collections.Generic;
using System.Text;
using Muuvis.DomainModel;

namespace Muuvis.Taste.DomainModel
{
    public class Suggestion : IEntity
    {
        private float _affinity;

        public Suggestion(string id, string movieId)
        {
            if (string.IsNullOrWhiteSpace(movieId)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(movieId));
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            if (id.Length > 36) throw new ArgumentException("Id cannot exceed 36 characters.", nameof(id));
            if (movieId.Length > 36) throw new ArgumentException("MovieId cannot exceed 36 characters.", nameof(movieId));

            Id = id;
            MovieId = movieId;
        }

        public string Id { get; }

        public string MovieId { get; }

        public float Affinity
        {
            get => _affinity;
            set
            {
                if (value < 0 && value > 1) throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 1");
                _affinity = value;
            }
        }
    }
}
