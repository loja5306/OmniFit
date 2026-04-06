using Microsoft.Extensions.Diagnostics.HealthChecks;
using OmniFit.Infrastructure.Data;

namespace OmniFit.Api.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public const string Name = "Database";

        private readonly AppDbContext _context;
        private readonly ILogger<DatabaseHealthCheck> _logger;

        public DatabaseHealthCheck(AppDbContext context, ILogger<DatabaseHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _context.Database.CanConnectAsync(cancellationToken))
                {
                    _logger.LogError("Database is unhealthy");
                    return HealthCheckResult.Unhealthy("Database is unhealthy");
                }
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                const string errorMessage = "Database is unhealthy";
                _logger.LogError(e, errorMessage);
                return HealthCheckResult.Unhealthy(errorMessage, e);
            }
        }
    }
}