using MyFinancialHub.Import.Domain.Entities;
using MyFinancialHub.Infra.Events.Consumers;

namespace MyFinancialHub.Import.Infra.Events.Consumers
{
    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddImportBalanceDataConsumer(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConsumer<ImportDataEvent?>, ImportBalanceDataConsumer>();
        }
    }
}
