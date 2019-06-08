using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muuvis.Depedencies;

namespace Muuvis.Web
{
    /// <summary>
    /// IWebHost implementation which wraps another one and checks dependencies before to start the original implementation
    /// </summary>
    public class DependenciesHost : IWebHost
    {
        private readonly IWebHost _originalHost;
        private readonly DependenciesConfiguration _configuration;

        public DependenciesHost(IWebHost originalHost, DependenciesConfiguration configuration)
        {
            _configuration = configuration;
            _originalHost = originalHost;
        }

        public IFeatureCollection ServerFeatures => _originalHost.ServerFeatures;

        public IServiceProvider Services => _originalHost.Services;

        public void Dispose()
        {
            _originalHost.Dispose();
        }

        public void Start()
        {
            Check().GetAwaiter().GetResult();
            _originalHost.Start();
        }

        public async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Check();
            await _originalHost.StartAsync(cancellationToken);
        }

        private async Task Check()
        {
            await Task.WhenAll(_configuration.Checkers.Select(c => c.WaitForReady()));
            Services.GetRequiredService<ILogger<DependenciesHost>>().LogInformation("All dependencies are ready");
        }

        public Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _originalHost.StopAsync(cancellationToken);
        }
    }
}
