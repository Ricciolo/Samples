using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Industria4.Http
{
	public class ServiceOptions
	{

        private static readonly Regex Name = new Regex("([^\\.]+)\\.(?=HttpClient)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public Dictionary<string, Uri> BaseUri { get; } = new Dictionary<string, Uri>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the uri based on the assembly which contains the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
	    public Uri GetFor<T>()
        {
            var match = Name.Match(typeof(T).Assembly.GetName().Name);
            if (!match.Success || match.Groups.Count < 1 || !match.Groups[1].Success)
                throw new ArgumentException($"Cannot find the key for type {typeof(T)}");

            string key = match.Groups[1].Value;

            if (!BaseUri.TryGetValue(key, out Uri uri))
	        {
                throw new ArgumentException($"Cannot find service uri with key {key}");
	        }

	        return uri;
	    }
	}
}
