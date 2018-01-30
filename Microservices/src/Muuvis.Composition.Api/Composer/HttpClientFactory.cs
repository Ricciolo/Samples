using System;
using System.Diagnostics;
using System.Net.Http;

namespace Muuvis.Composition.Api.Composer
{
    public class HttpClientFactory
    {
        private HttpClient _client;
        private readonly Stopwatch _creationWatch = new Stopwatch();
        private static readonly TimeSpan MaxElapsed = TimeSpan.FromMinutes(15);

        public HttpClient Get()
        {
            if (_client == null || _creationWatch.Elapsed > MaxElapsed)
            {
                lock (this)
                {
                    if (_client == null || _creationWatch.Elapsed > MaxElapsed)
                    {
                        _creationWatch.Restart();
                        _client = new HttpClient();
                    }
                }
            }

            return _client;
        }
    }
}