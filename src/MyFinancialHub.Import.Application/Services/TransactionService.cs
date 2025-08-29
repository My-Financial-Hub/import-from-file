using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Application.Services
{
    internal class TransactionService(ITransactionsRepository repository, ILogger<TransactionService> logger)
    {
        private readonly ITransactionsRepository repository = repository;
        private readonly ILogger<TransactionService> logger = logger;

        public async Task InsertAsync(IEnumerable<Transaction> transactions, string balanceName)
        {
            this.logger.BeginScope("Add Transactions");
            if (transactions == null || !transactions.Any())
            {
                this.logger.LogWarning("No transactions provided to add.");
                return;
            }
            await this.repository.AddAsync(balanceName, transactions);

            await this.repository.CommitAsync();

            this.logger.LogInformation("All transactions processed and committed successfully.");
        }
    }
}
