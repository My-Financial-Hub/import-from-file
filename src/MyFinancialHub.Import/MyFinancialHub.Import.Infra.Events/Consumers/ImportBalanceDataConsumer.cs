using Microsoft.Extensions.Logging;
using MyFinancialHub.Import.Domain.Entities;
using MyFinancialHub.Infra.Events.Consumers;

namespace MyFinancialHub.Import.Infra.Events.Consumers
{
    public class ImportBalanceDataConsumer(
        ILogger<ImportBalanceDataConsumer> logger
    ) : IConsumer<ImportDataEvent?>
    {
        private readonly ILogger<ImportBalanceDataConsumer> logger = logger;

        public async Task<ImportDataEvent?> ConsumeAsync()
        {
            return null;
        }
    }
}
