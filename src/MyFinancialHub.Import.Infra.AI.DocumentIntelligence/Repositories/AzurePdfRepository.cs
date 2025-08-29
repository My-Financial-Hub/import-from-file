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
            this.logger.LogDebug("Starting PDF import.");
            if (fileStream == null)
            {
                var exception = new ArgumentNullException(nameof(fileStream), "File stream cannot be null.");
                this.logger.LogError(exception, "Failed to import PDF due to null file stream.");
                throw exception;
            }

            this.logger.LogDebug("Analyzing document with model ID: {ModelId}", this.configs.ModelId);

            this.logger.LogTrace("Reading file stream into BinaryData.");
            var bytesSource = await BinaryData.FromStreamAsync(fileStream);
            this.logger.LogTrace("File stream read into BinaryData successfully.");

            this.logger.LogDebug("Sending document to Azure Document Intelligence for analysis.");
            var operation = await this.client.AnalyzeDocumentAsync(
                WaitUntil.Completed, 
                this.configs.ModelId, 
                bytesSource
            );
            this.logger.LogDebug("Document analysis completed.");

            if (operation == null || operation.Value == null)
            {
                var exception = new InvalidOperationException("Document analysis operation failed or returned null.");
                this.logger.LogError(exception, "Failed to analyze document.");
                throw exception;
            }

            this.logger.LogTrace("Mapping analyzed data to PdfImportData.");
            var pdfData = dataMapper.Map(operation.Value);
            this.logger.LogTrace("Mapping completed successfully.");

            this.logger.LogDebug(
                "PDF import completed successfully with {CategoryCount} categories, {BalanceCount} balances, and {TransactionCount} transactions.",
                pdfData.Categories.Count, pdfData.Balances.Count, pdfData.Transactions.Count
            );
            return pdfData;
        }
    }
}
