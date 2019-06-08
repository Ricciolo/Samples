using System;
using System.Globalization;
using Muuvis.DomainModel;

namespace Muuvis.Catalog.DomainModel
{
    public class Movie : IEntity
    {
        private int _year;
        private bool _isDeleted;

        public Movie(string id, CultureInfo originalCulture, string originalTitle)
        {
            if (originalCulture == null) throw new ArgumentNullException(nameof(originalCulture));
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            if (string.IsNullOrWhiteSpace(originalTitle)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(originalTitle));

            Id = id;
            Translation = new TitleTranslation(originalCulture, originalTitle);
            Year = 1900;
        }

        public string Id { get; private set; }

        public TitleTranslation Translation { get; }

        public string OriginalTitle
        {
            get => Translation.Original;
            set => Translation.Original = value;
        }

        public int Year
        {
            get => _year;
            set
            {
                if (value < 1900) throw new ArgumentException($"{nameof(Year)} cannot be less than 1900");
                _year = value;
            }
        }

        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                if (_isDeleted && !value) throw new InvalidOperationException($"{nameof(IsDeleted)} cannot be set to false");
                _isDeleted = value;
            }
        }
    }
}
