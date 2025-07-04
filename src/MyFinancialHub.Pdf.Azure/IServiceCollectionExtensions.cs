using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyFinancialHub.Pdf.Azure.Configurations;
using MyFinancialHub.Pdf.Azure.Mappers;
using MyFinancialHub.Pdf.Azure.Repositories;

namespace MyFinancialHub.Pdf.Azure
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureRepositories(this IServiceCollection services)
        {
            return services
                .AddConfigurations()
                .AddMappers()
                .AddAzureDocumentIntelligence()
                .AddRepositories();
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services)
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

        private static IServiceCollection AddAzureDocumentIntelligence(this IServiceCollection services)
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
            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddScoped<TransactionMapper>();
            services.AddScoped<BalanceMapper>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPdfDocumentRepository, AzurePdfRepository>();
            return services;
        }
    }
}
