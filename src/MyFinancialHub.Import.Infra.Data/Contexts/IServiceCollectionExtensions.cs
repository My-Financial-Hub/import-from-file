using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace MyFinancialHub.Import.Infra.Data.Contexts
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("infra_data_appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("infra_data_appsettings.development.json", optional: true, reloadOnChange: false)
                .Build();

            services.AddDbContext<FinancialHubContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("CoreDatabase"),
                    config => {
                        config
                            .MigrationsHistoryTable("migrations")
                            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1), null);
                    }
                )
            );
            return services;
        }
    }
}
