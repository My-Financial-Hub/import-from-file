using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Services;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Configurations;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Repositories;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentInteligence(this IServiceCollection services)
        {
            return services
                .AddConfigurations()
                .AddMappers()
                .AddServices()
                .AddRepositories();
        }
    }
}
