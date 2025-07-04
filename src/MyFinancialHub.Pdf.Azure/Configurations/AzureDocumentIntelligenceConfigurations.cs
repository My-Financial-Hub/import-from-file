namespace MyFinancialHub.Pdf.Azure.Configurations
{
    internal class AzureDocumentIntelligenceConfigurations
    {
        public string Endpoint { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        public string ModelId { get; init; } = "prebuilt-layout"; // Default model ID
    }
}
