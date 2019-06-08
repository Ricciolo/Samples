using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Muuvis.Depedencies
{
    /// <summary>
    /// Check if a TCP connection to a host works
    /// </summary>
    public class TcpCheck : IDependencyChecker
    {
        private readonly ILogger<TcpCheck> _logger;
        private readonly string _host;
        private readonly int _port;

        public TcpCheck(ILogger<TcpCheck> logger, string host, int port)
        {
            _logger = logger;
            _host = host;
            _port = port;
        }

        public async Task WaitForReady()
        {
            int r = 0;
            while (true)
            {
                try
                {
                    using (var client = new TcpClient())
                    {
                        _logger.LogInformation("Checking tcp dependency. Endpoint: {host}:{port}", _host, _port);
                        await client.ConnectAsync(_host, _port);
                        return;
                    }
                }
                catch
                {
                    r++;
                    _logger.LogWarning("Tcp dependency unreachable. Endpoint: {host}:{port}", _host, _port);
                    await Task.Delay(1000);
                }
            }
        }
    }
}
