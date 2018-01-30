using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Helpers;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Muuvis.Composition.Api.Composer
{
    public abstract class HttpAppender : IAppender
    {
        private readonly HttpClientFactory _httpClientFactory;

        protected HttpAppender(HttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string Template { get; set; }

        public async Task<IAppenderResult> GetAsync(string url, CancellationToken token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await _httpClientFactory.Get().SendAsync(request, token);

            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            return GetResult(json);
        }

        protected abstract IAppenderResult GetResult(string json);
    }

    public class HttpSingleAppender : HttpAppender
    {
        public HttpSingleAppender(HttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        protected override IAppenderResult GetResult(string json)
        {
            var o = JsonConvert.DeserializeObject<ExpandoObject>(json);
            return new AppenderSingleResult(o);
        }
    }

    public class HttpListAppender : HttpAppender
    {
        public HttpListAppender(HttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        protected override IAppenderResult GetResult(string json)
        {
            var o = JsonConvert.DeserializeObject<ExpandoObject[]>(json);
            return new AppenderListResult(o);
        }
    }
}
