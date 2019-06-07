using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Industria4.Depedencies
{
    /// <summary>
    /// Chech if connection to a SQL Server is available
    /// </summary>
    public class SqlConnectionCheck : IDependencyChecker
    {
        private readonly ILogger<SqlConnectionCheck> _logger;
        private readonly string _connectionString;

        public SqlConnectionCheck(ILogger<SqlConnectionCheck> logger, string connectionString)
        {
            _logger = logger;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";
            _connectionString =  builder.ToString();
        }

        public async Task WaitForReady()
        {
            int r = 0;
            while (true)
            {
                try
                {
                    SqlConnection.ClearAllPools();
                    _logger.LogWarning("Checking Sql Server dependency. Connection string: {connectionString}", _connectionString);
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        await connection.OpenAsync();
                    }
                    return;
                }
                catch
                {
                    r++;
                    _logger.LogWarning("Sql Server dependency unreachable. Connection string: {connectionString}", _connectionString);
                    await Task.Delay(1000);
                }
            }
        }
    }
}
