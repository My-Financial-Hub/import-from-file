using Microsoft.Extensions.DependencyInjection;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountsRepository>();
            services.AddScoped<IBalanceRepository, BalancesRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddScoped<ICategoryRepository, CategoriesRepository>();
            return services;
        }
    }
}
