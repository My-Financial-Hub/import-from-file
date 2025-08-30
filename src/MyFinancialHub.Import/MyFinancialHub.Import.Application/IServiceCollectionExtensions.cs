using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Import.Application.Handlers;
using MyFinancialHub.Import.Application.Services;

namespace MyFinancialHub.Import.Application
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddCqrs()
                .AddHandlers()
                .AddServices();
        }
    }
}
