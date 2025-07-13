using Microsoft.Extensions.Logging;
using MyFinancialHub.Import.Domain.Interfaces.Services;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Repositories;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Services
{
    internal class AzurePdfServices(
        ImportDataMapper mapper, 
        AzurePdfRepository repository, 
        ILogger<AzurePdfServices> logger
        ) : IImportDataService
    {
        private readonly AzurePdfRepository repository = repository;
        private readonly ILogger<AzurePdfServices> logger = logger;

        public async Task<ImportData> ImportAsync(Stream fileStream)
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
