using MyFinancialHub.Import.Infra.Data.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace MyFinancialHub.Import.Infra.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlRepositories(this IServiceCollection services)
        {
            return services
                .AddDbContext()
                .AddRepositories()
                .AddRepositoriesMapper();
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("infra_data_appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("infra_data_appsettings.development.json", optional: true, reloadOnChange: false)
                .Build();

            services.AddDbContext<FinancialHubContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("CoreDatabase"),
                    x => x.MigrationsHistoryTable("migrations")
                )
            );
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountsRepository>();
            services.AddScoped<IBalanceRepository, BalancesRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddScoped<ICategoryRepository, CategoriesRepository>();
            return services;
        }

        private static IServiceCollection AddRepositoriesMapper(this IServiceCollection services)
        {
            services.AddScoped<AccountMapper>();
            services.AddScoped<BalanceMapper>();
            //services.AddScoped<TransactionMapper>();
            //services.AddScoped<CategoryMapper>();
            return services;
        }
    }
}
