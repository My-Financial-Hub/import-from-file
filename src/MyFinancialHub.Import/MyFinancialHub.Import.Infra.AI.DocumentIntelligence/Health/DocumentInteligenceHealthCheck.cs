using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Health
{
    internal class DocumentInteligenceHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isHealthy = true;

            if (isHealthy)
            {
                return HealthCheckResult.Healthy();
            }

            return new HealthCheckResult(context.Registration.FailureStatus);
        }
    }
}
