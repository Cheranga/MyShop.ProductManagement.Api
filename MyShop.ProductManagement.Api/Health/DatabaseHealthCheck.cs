using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyShop.ProductManagement.Api.Configs;

namespace MyShop.ProductManagement.Api.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly DatabaseConfig _config;

        public DatabaseHealthCheck(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var connectionString = _config.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return HealthCheckResult.Unhealthy("Empty or null connection string");
            }

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                }

                return HealthCheckResult.Healthy();
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy(exception: exception);
            }
        }
    }

    public class SomeOtherCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("A description about why it's not working."));
        }
    }
}