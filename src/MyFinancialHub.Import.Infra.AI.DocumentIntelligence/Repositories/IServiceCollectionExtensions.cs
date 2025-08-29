using Azure;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Configurations;
using Microsoft.Extensions.Options;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Repositories
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(services =>
            {
                var options = services.GetRequiredService<IOptions<AzureDocumentIntelligenceConfigurations>>();
                var key = options.Value.ApiKey;
                return new AzureKeyCredential(key);
            });
            services.AddScoped(services =>
            {
                var options = services.GetRequiredService<IOptions<AzureDocumentIntelligenceConfigurations>>();
                var endpoint = new Uri(options.Value.Endpoint);
                var credential = services.GetRequiredService<AzureKeyCredential>();
                return new DocumentIntelligenceClient(endpoint, credential);
            });

            services.AddScoped<AzurePdfRepository>();

            return services;
        }
    }
}
