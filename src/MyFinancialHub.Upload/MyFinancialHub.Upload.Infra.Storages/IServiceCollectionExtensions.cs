using MyFinancialHub.Upload.Infra.Storages.Services.AzureBlobStorage;

namespace MyFinancialHub.Upload.Infra.Storages
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFileStorage(this IServiceCollection services)
        {
            return services
                .AddAzureBlobStorage();
        }
    }
}
