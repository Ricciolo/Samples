using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Simple.OData.Client;

namespace ODataSampleClient
{
    public class GenericClient<T>
        where T : class
    {
        private readonly ODataClient _client;

        public GenericClient(HttpClient client)
        {
            _client = GetClient(client);
        }

        public IBoundClient<T> Client => _client.For<T>();

        private ODataClient GetClient(HttpClient httpClient)
        {
            var baseUri = new Uri("https://localhost:44311/");
            if (httpClient.BaseAddress != baseUri)
            {
                httpClient.BaseAddress = baseUri;
            }

            var settings = new ODataClientSettings(httpClient, new Uri("/odata", UriKind.Relative));
            var client = new ODataClient(settings);

            return client;
        }

    }
}
