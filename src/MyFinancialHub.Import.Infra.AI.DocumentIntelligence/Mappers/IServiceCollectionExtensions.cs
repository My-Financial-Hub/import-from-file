using Microsoft.Extensions.DependencyInjection;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddScoped<ImportDataMapper>();

            services.AddScoped<PdfDataMapper>();
            services.AddScoped<PdfCategoryMapper>();
            services.AddScoped<PdfBalanceMapper>();
            services.AddScoped<PdfTransactionMapper>();

            return services;
        }
    }
}
