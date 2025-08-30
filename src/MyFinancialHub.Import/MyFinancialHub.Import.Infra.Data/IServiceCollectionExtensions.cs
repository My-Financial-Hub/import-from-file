using Microsoft.Extensions.DependencyInjection;
using MyFinancialHub.Import.Infra.Data.Health;

namespace MyFinancialHub.Import.Infra.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlRepositories(this IServiceCollection services)
        {
            return services
                .AddHealthCheck()
                .AddDbContext()
                .AddRepositories()
                .AddMappers();
        }
    }
}
