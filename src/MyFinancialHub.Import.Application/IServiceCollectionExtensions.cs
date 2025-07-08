using Microsoft.Extensions.DependencyInjection;
using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Application.CQRS.Handlers.Commands;
using MyFinancialHub.Import.Application.Handlers.ImportPdfFile;
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


        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<ImportPdfFileCommand>, ImportPdfFileCommandHandler>();
            return services;
        } 

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<BalanceService>();
            return services;
        } 
    }
}
