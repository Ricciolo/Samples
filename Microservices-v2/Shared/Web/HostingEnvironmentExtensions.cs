using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    ///     Extensions for <see cref="IHostingEnvironment" />
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        ///     Gets if current environment is unix type
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static bool IsUnix(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        public static bool IsKubernetes(this IHostingEnvironment hostingEnvironment)
        {
            return !String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT"));
        }
    }
}