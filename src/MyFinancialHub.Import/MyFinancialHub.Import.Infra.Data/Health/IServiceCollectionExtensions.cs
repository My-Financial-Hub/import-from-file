using Microsoft.Extensions.DependencyInjection;

namespace MyFinancialHub.Import.Infra.Data.Health
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddDbContextCheck<FinancialHubContext>();

            return services;
        }
    }
}
