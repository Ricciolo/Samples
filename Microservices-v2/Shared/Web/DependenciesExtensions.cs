using System;
using System.Collections.Generic;
using System.Text;
using Industria4.Web;
using Industria4.Depedencies;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Extensions for IWebHost which allow to apply depedency check before running the host
    /// </summary>
    public static class DependenciesExtensions
    {
        public static IWebHost CheckDependencies(this IWebHost host, Action<DependenciesConfiguration> configuration)
        {
            var c = new DependenciesConfiguration(host.Services);
            configuration(c);
            return new DependenciesHost(host, c);
        }
    }
}