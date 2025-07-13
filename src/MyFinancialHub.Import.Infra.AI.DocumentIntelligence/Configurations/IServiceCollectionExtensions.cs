using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Configurations
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: false)
                .Build();

            services.Configure<AzureDocumentIntelligenceConfigurations>(
                configuration.GetSection("Azure:CognitiveServices:DocumentIntelligence"));

            return services;
        }
    }
}
