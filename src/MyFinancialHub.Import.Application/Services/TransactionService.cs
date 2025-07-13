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

            this.logger.LogInformation("Starting to process {TransactionCount} transactions for balance: {BalanceName}", transactions.Count(), balanceName);
            foreach (var transaction in transactions)
            {
                this.logger.LogInformation("Processing transaction: {TransactionDescription}", transaction.Description);
                await this.repository.AddAsync(transaction, balanceName);
                this.logger.LogInformation("Transaction {TransactionDescription} added successfully.", transaction.Description);
            }

            this.logger.LogInformation("Committing all transactions.");
            await this.repository.CommitAsync();
            this.logger.LogInformation("All transactions processed successfully.");
        }
    }
}
