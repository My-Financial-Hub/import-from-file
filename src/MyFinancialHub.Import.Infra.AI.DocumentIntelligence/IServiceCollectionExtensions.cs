using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyFinancialHub.Import.Domain.Interfaces.Services;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Configurations;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Services;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentInteligence(this IServiceCollection services)
        {
            return services
                .AddConfigurations()
                .AddMappers()
                .AddAzureDocumentIntelligence()
                .AddServices();
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

            services.AddScoped<PdfDataMapper>();
            services.AddScoped<PdfCategoryMapper>();
            services.AddScoped<PdfBalanceMapper>();
            services.AddScoped<PdfTransactionMapper>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBalanceImportService, AzurePdfServices>();
            services.AddScoped<AzurePdfRepository>();
            return services;
        }
    }
}
