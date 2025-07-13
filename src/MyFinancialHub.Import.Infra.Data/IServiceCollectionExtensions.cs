using Microsoft.Extensions.DependencyInjection;

namespace MyFinancialHub.Import.Infra.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlRepositories(this IServiceCollection services)
        {
            return services
                .AddDbContext()
                .AddRepositories()
                .AddMappers();
        }
    }
}
