using System;
using MassTransit;

namespace Muuvis.Taste.DomainModel
{
    public class Suggestion
    {
        private float _affinity;

        public Suggestion(string movieId) : this(NewId.Next().ToString("D"), movieId)
        {
        }

        public Suggestion(string id, string movieId)
        {
            if (string.IsNullOrWhiteSpace(movieId)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(movieId));
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));

            Id = id;
            MovieId = movieId;
        }

        public string Id { get; private set; }

        public string MovieId { get; private set; }

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
