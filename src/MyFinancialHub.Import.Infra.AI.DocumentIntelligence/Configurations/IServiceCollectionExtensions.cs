using Microsoft.Extensions.Configuration;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Configurations
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("doc_int_appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("doc_int_appsettings.development.json", optional: true, reloadOnChange: false)
                .Build();

            services.Configure<AzureDocumentIntelligenceConfigurations>(
                configuration.GetSection("Azure:CognitiveServices:DocumentIntelligence")
            );

            return services;
        }
    }
}
