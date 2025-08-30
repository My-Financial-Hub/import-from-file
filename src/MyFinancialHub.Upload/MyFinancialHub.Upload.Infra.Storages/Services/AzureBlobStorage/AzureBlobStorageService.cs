using MyFinancialHub.Upload.Domain;
using MyFinancialHub.Upload.Domain.Interfaces.Services;

namespace MyFinancialHub.Upload.Infra.Storages.Services.AzureBlobStorage
{
    internal class AzureBlobStorageService(
        BlobContainerClient client,
        ILogger<AzureBlobStorageService> logger
    ) : IUploadDataService
    {
        private readonly BlobContainerClient client = client;
        private readonly ILogger<AzureBlobStorageService> logger = logger;

        public async Task<UploadData> ProcessUploadAsync(string dataName, Stream data)
        {
            using var _ = this.logger.BeginScope("ProcessUploadAsync");
            this.logger.LogDebug("Starting upload of {DataName} to Azure Blob Storage", dataName);

            this.logger.LogTrace("Uploading data to blob container: {ContainerName}", client.Name);
            var blob = client.GetBlobClient("test");
            this.logger.LogDebug("Uploading data to blob: {BlobName}", blob.Name);

            this.logger.LogTrace("Uploading data...");
            var response = await blob.UploadAsync(dataName);
            this.logger.LogTrace("Upload completed with status: {Status}", response.GetRawResponse().Status);

            this.logger.LogDebug("Upload of {DataName} completed successfully", dataName);
            return new UploadData
            {
                FileUrl = response.GetRawResponse().ToString()
            };
        }
    }
}
