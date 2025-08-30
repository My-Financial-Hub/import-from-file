using MyFinancialHub.Application.CQRS.Handlers.Commands;
using MyFinancialHub.Upload.Application.Handlers.UploadPdfFile;

namespace MyFinancialHub.Upload.Application.Handlers
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandHandler<UploadPdfFileCommand>, UploadPdfFileCommandHandler>();
        } 
    }
}
