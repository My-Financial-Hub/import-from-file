using Azure;
using Microsoft.Extensions.Logging;
using MyFinancialHub.Pdf.Azure.Mappers;

namespace MyFinancialHub.Pdf.Azure.Repositories
{
    internal class AzurePdfRepository
        (DocumentIntelligenceClient client, BalanceMapper mapper, ILogger<AzurePdfRepository> logger)
        : IPdfDocumentRepository
    {
        private readonly DocumentIntelligenceClient client = client;
        private readonly BalanceMapper mapper = mapper;
        private readonly ILogger<AzurePdfRepository> logger = logger;

        private const string modelId = "prebuilt-layout";

        public async Task<IEnumerable<Balance>> AnalyzeAsync(Stream fileStream)
        {
            this.logger.LogInformation("Starting document analysis with model ID: {ModelId}", modelId);

            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream), "Stream cannot be null.");

            var bytesSource = await BinaryData.FromStreamAsync(fileStream);
            var operation = await this.client.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, bytesSource);

            this.logger.LogInformation("Document analysis completed");
            return this.mapper.MapFromTransactionTables(operation.Value.Tables);
        }
    }
}
