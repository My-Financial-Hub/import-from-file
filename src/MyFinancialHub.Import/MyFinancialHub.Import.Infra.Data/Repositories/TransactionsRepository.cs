using MyFinancialHub.Import.Domain.Entities.Transactions;
using System.Linq;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class TransactionsRepository(
        FinancialHubContext context, 
        TransactionMapper mapper,
        ILogger<TransactionsRepository> logger
    ) : BaseRepository(context, logger), ITransactionsRepository
    {
        private readonly TransactionMapper mapper = mapper;

        public async Task AddAsync(string balanceName, Transaction transaction)
        {
            using var _ = this.logger.BeginScope("Adding transaction to balance");
            if (string.IsNullOrWhiteSpace(balanceName))
            {
                var exception = new ArgumentException("Balance name cannot be null or empty.", nameof(balanceName));
                this.logger.LogError(exception, "Failed to add transaction due to invalid balance name");
                throw exception;
            }
            if (transaction == null)
            {
                var exception = new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
                this.logger.LogError(exception, "Failed to add transaction due to null transaction");
                throw exception;
            }
            this.logger.LogDebug("Adding transaction to balance {BalanceName}", balanceName);

            this.logger.LogDebug("Retrieving balance by name {BalanceName}", balanceName);
            var balance = await this.context.Balances
                .AsNoTracking()
                .FirstAsync(balance => balance.Name == balanceName);
            this.logger.LogDebug("Retrieved balance with ID {BalanceId}", balance.Id);

            this.logger.LogDebug("Retrieving category by name {CategoryName}", transaction.Category?.Name);
            var categoryName = transaction.Category?.Name;
            var category = await this.context.Categories
                .AsNoTracking()
                .FirstAsync(a => a.Name == categoryName);
            this.logger.LogDebug("Retrieved category with ID {CategoryId}", category.Id);

            this.logger.LogTrace("Mapping transaction to entity");
            var transactionEntity = this.mapper.Map(transaction);
            this.logger.LogTrace("Mapped transaction to entity");

            this.logger.LogTrace("Setting foreign keys for transaction entity");
            transactionEntity.CategoryId = category.Id.GetValueOrDefault();
            transactionEntity.BalanceId = balance.Id.GetValueOrDefault();
            this.logger.LogTrace("Set foreign keys for transaction entity");

            this.logger.LogTrace("Adding transaction entity to context");
            var now = DateTimeOffset.Now;
            transactionEntity.CreationTime = now;
            transactionEntity.UpdateTime = now;
            this.logger.LogTrace("Added transaction entity to context");

            this.logger.LogDebug("Adding transaction to balance {BalanceName}", balanceName);
            await this.context.Transactions.AddAsync(transactionEntity);
            this.logger.LogDebug("Added transaction to balance {BalanceName}", balanceName);
        }

        public async Task AddAsync(string balanceName, IEnumerable<Transaction> transactions)
        {
            this.logger.BeginScope("Add Transactions");
            if (transactions == null || !transactions.Any())
            {
                var exception = new ArgumentNullException(nameof(transactions), "No transactions provided to add.");
                this.logger.LogError(exception, "No transactions provided to add");
                throw exception;
            }

            this.logger.LogDebug("Retrieving balance by name {BalanceName}", balanceName);
            var balance = await this.context.Balances
                .AsNoTracking()
                .FirstAsync(balance => balance.Name == balanceName);
            this.logger.LogDebug("Retrieved balance with ID {BalanceId}", balance.Id);

            var categoryNames = transactions
                .Where(t => !string.IsNullOrWhiteSpace(t.Category?.Name))
                .Select(t => t.Category!.Name)
                .Distinct()
                .ToArray();

            var categories = await this.context.Categories
                .AsNoTracking()
                .Where(c => categoryNames.Contains(c.Name))
                .ToDictionaryAsync(c => c.Name, c => c);

            this.logger.LogDebug("Starting to process transactions for balance: {BalanceName}", balanceName);

            foreach (var transaction in transactions)
            {
                this.logger.LogTrace("Mapping transaction to entity");
                var transactionEntity = this.mapper.Map(transaction);
                this.logger.LogTrace("Mapped transaction to entity");

                this.logger.LogTrace("Setting foreign keys for transaction entity");
                transactionEntity.CategoryId = categories[transaction.Category!.Name].Id.GetValueOrDefault();
                transactionEntity.BalanceId = balance.Id.GetValueOrDefault();
                this.logger.LogTrace("Set foreign keys for transaction entity");

                this.logger.LogTrace("Adding transaction entity to context");
                var now = DateTimeOffset.Now;
                transactionEntity.CreationTime = now;
                transactionEntity.UpdateTime = now;
                this.logger.LogTrace("Added transaction entity to context");

                this.logger.LogDebug("Adding transaction to balance {BalanceName}", balanceName);
                await this.context.Transactions.AddAsync(transactionEntity);
                this.logger.LogDebug("Added transaction to balance {BalanceName}", balanceName);
            }

            this.logger.LogDebug("Processed all transactions for balance: {BalanceName}", balanceName);
        }
    }
}
