using Microsoft.Extensions.DependencyInjection;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddScoped<AccountMapper>();
            services.AddScoped<BalanceMapper>();
            services.AddScoped<TransactionMapper>();
            services.AddScoped<CategoryMapper>();
            return services;
        }
    }
}
