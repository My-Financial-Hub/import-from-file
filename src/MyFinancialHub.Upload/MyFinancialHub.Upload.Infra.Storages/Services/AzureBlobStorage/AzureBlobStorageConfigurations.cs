namespace MyFinancialHub.Upload.Infra.Storages.Services.AzureBlobStorage
{
    internal class AzureBlobStorageConfigurations
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
