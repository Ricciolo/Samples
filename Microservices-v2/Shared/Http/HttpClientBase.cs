using Pathoschild.Http.Client;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pathoschild.Http.Client.Extensibility;

namespace Muuvis.Http
{
	public abstract class HttpClientBase : IDisposable, IFireAndForget
    {
        protected IServiceProvider ServiceProvider { get; }

        public Uri Uri { get; }

		protected FluentClient Client { get; }

        protected HttpClientBase(IServiceProvider serviceProvider, Uri uri)
		{
		    ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		    Uri = uri ?? throw new ArgumentNullException(nameof(uri));
			Client = new FluentClient(Uri);

            // Set custom error handling
		    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            Client.Filters.Add(new LoggingFilter(loggerFactory.CreateLogger(GetType())));
            Client.Filters.Remove<DefaultErrorFilter>();
		    Client.Filters.Add(new NotFoundFilter());

            // Set automatic retry
            Client.SetRequestCoordinator(
		        maxRetries: 3,
		        shouldRetry: r => !r.IsSuccessStatusCode && r.StatusCode != HttpStatusCode.NotFound && r.StatusCode != HttpStatusCode.Conflict,
		        getDelay: (attempt, response) => TimeSpan.FromSeconds(Math.Pow(attempt, 2) / 4d) // 0.25, 1, 2,25
            );

		    ((IFireAndForget) this).Enabled = false;
		}

		public void Dispose()
		{
			Client?.Dispose();
		}

        bool IFireAndForget.Enabled
        {
            get => Client.BaseClient.DefaultRequestHeaders.Contains("x-async");
            set
            {
                if (value == ((IFireAndForget) this).Enabled) return;

                if (value)
                {
                    Client.BaseClient.DefaultRequestHeaders.Add("x-async", "true");
                }
                else
                {
                    Client.BaseClient.DefaultRequestHeaders.Remove("x-async");
                }
            }
        }

        private class NotFoundFilter : IHttpFilter
        {
            public void OnRequest(IRequest request)
            {

            }

            public void OnResponse(IResponse response, bool httpErrorAsException)
            {
                if (httpErrorAsException && !response.Message.IsSuccessStatusCode)
                {
                    if (response.Message.StatusCode != HttpStatusCode.NotFound)
                        throw new ApiException(response, $"The API call {response.Message.RequestMessage?.RequestUri} failed with status code {response.Message.StatusCode}: {response.Message.ReasonPhrase}");

                    // In case of 404 we return an empty content which is read as null
                    response.Message.Content = new ByteArrayContent(Array.Empty<byte>()) { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };
                }
            }
        }

        private class LoggingFilter : IHttpFilter
        {
            private readonly ILogger _logger;

            public LoggingFilter(ILogger logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public void OnRequest(IRequest request)
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Calling {request}", request.Message);
                }
            }

            public void OnResponse(IResponse response, bool httpErrorAsException)
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Response received from {uri}: {response}", response.Message.RequestMessage.RequestUri, response.Message);
                }
            }
        }
    }
}
