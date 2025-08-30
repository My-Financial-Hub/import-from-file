using Microsoft.Extensions.DependencyInjection;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Health
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            return services;
        }
    }
}
