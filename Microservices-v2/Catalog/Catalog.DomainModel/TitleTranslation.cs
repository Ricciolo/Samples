using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Humanizer;

namespace Muuvis.Catalog.DomainModel
{
    public class TitleTranslation : IDictionary<CultureInfo, string>
    {
        public static CultureInfo Default = new CultureInfo("en-US");

        private CultureInfo _originalCulture = Default;

        private readonly Dictionary<CultureInfo, string> _inner = new Dictionary<CultureInfo, string>();

        public TitleTranslation(CultureInfo originalCulture, string original)
        {
            OriginalCulture = originalCulture;
            Original = original;
        }

        public void Add(CultureInfo key, string value) => _inner[key] = value;

        public bool ContainsKey(CultureInfo key) => _inner.ContainsKey(key);

        public bool Remove(CultureInfo key)
        {
            if (!Equals(key, OriginalCulture)) throw new ArgumentException("Cannot remove original culture", nameof(key));

            return _inner.Remove(key);
        }

        public bool TryGetValue(CultureInfo key, out string value) => _inner.TryGetValue(key, out value);

        public string this[CultureInfo culture]
        {
            get => _inner[culture];
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
                if (value.Length > 255) throw new ArgumentException("Value cannot exceed 255 characters.", nameof(value));

                _inner[culture] = value.Titleize();
            }
        }

        public ICollection<CultureInfo> Keys => _inner.Keys;

        public ICollection<string> Values => _inner.Values;

        public CultureInfo OriginalCulture
        {
            get => _originalCulture;
            set => _originalCulture = value ?? throw new ArgumentNullException(nameof(value), "Cannot set to null");
        }

        public string Original
        {
            get => _inner.ContainsKey(OriginalCulture) ? this[OriginalCulture] : String.Empty;
            set => this[OriginalCulture] = value;
        }

        public IEnumerator<KeyValuePair<CultureInfo, string>> GetEnumerator() => _inner.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<CultureInfo, string> item) => this[item.Key] = item.Value;

        public void Clear() => throw new NotSupportedException();

        public bool Contains(KeyValuePair<CultureInfo, string> item) => _inner.ContainsKey(item.Key);

        void ICollection<KeyValuePair<CultureInfo, string>>.CopyTo(KeyValuePair<CultureInfo, string>[] array, int arrayIndex) => throw new NotSupportedException();

        public bool Remove(KeyValuePair<CultureInfo, string> item) => Remove(item.Key);

        public int Count => _inner.Count;

        public bool IsReadOnly => false;
    }
}
