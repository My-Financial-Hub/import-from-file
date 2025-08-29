using MyFinancialHub.Import.Application.Handlers.ImportPdfFile;

namespace MyFinancialHub.Import.Application.Handlers
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandHandler<ImportPdfFileCommand>, ImportPdfFileCommandHandler>();
        } 
    }
}
