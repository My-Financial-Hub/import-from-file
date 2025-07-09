using Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Configurations;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Repositories
{
    internal class AzurePdfRepository(
        DocumentIntelligenceClient client, 
        PdfDataMapper dataMapper,
        IOptions<AzureDocumentIntelligenceConfigurations> configs,
        ILogger<AzurePdfRepository> logger
        )
    {
        private readonly DocumentIntelligenceClient client = client;
        private readonly PdfDataMapper dataMapper = dataMapper;
        private readonly AzureDocumentIntelligenceConfigurations configs = configs.Value;
        private readonly ILogger<AzurePdfRepository> logger = logger;

        public async Task<PdfImportData> ImportAsync(Stream fileStream)
        {
            this.logger.LogInformation("Starting PDF import.");
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream), "File stream cannot be null.");

            var bytesSource = await BinaryData.FromStreamAsync(fileStream);
            var operation = await this.client.AnalyzeDocumentAsync(WaitUntil.Completed, this.configs.ModelId, bytesSource);

            return dataMapper.Map(operation.Value);
        }
    }
}
