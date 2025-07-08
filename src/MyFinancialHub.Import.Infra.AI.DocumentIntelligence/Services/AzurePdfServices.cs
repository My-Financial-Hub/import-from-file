using Microsoft.Extensions.Logging;
using MyFinancialHub.Import.Domain.Interfaces.Services;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Services
{
    internal class AzurePdfServices(
        BalanceMapper mapper, 
        AzurePdfRepository repository, 
        ILogger<AzurePdfServices> logger
        ) : IBalanceImportService
    {
        private readonly AzurePdfRepository repository = repository;
        private readonly ILogger<AzurePdfServices> logger = logger;

        public async Task<IEnumerable<Balance>> ImportAsync(Stream fileStream)
        {
            using var _ = logger.BeginScope("AzurePdfServices.ImportAsync");
            logger.LogInformation("Starting import of PDF file stream.");
            if (fileStream == null)
            {
                logger.LogError("File stream is null.");
                throw new ArgumentNullException(nameof(fileStream), "File stream cannot be null.");
            }

            logger.LogInformation("Calling repository to analyze PDF document.");
            var data = await repository.ImportAsync(fileStream);

            logger.LogInformation("Mapping PDF data to balances.");
            return mapper.Map(data);
        }
    }
}
