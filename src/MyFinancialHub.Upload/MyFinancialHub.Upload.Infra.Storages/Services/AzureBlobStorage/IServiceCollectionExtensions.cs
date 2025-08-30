using Microsoft.Extensions.Options;
using MyFinancialHub.Upload.Domain.Interfaces.Services;

namespace MyFinancialHub.Upload.Infra.Storages.Services.AzureBlobStorage
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddAzureBlobStorage(this IServiceCollection services)
        {
            services
                .AddScoped(sp =>
                {
                    var configuration = sp.GetRequiredService<IOptions<AzureBlobStorageConfigurations>>();
                    var connectionString = configuration.Value.ConnectionString;
                    var containerName = configuration.Value.ContainerName;
                    return new BlobContainerClient(connectionString, containerName);
                })
                .AddScoped<IUploadDataService, AzureBlobStorageService>();
            return services;
        }
    }
}
