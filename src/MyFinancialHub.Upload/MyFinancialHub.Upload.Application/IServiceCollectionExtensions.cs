using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Upload.Application.Handlers;

namespace MyFinancialHub.Upload.Application
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddCqrs()
                .AddHandlers();
        }
    }
}
