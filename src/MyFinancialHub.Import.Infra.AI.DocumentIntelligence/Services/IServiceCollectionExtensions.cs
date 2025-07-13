using Microsoft.Extensions.DependencyInjection;
using MyFinancialHub.Import.Domain.Interfaces.Services;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Services
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IImportDataService, AzurePdfServices>();
            return services;
        }
    }
}
