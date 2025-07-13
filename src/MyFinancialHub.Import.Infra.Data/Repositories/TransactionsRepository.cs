using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class TransactionsRepository(FinancialHubContext context, TransactionMapper mapper) : 
        BaseRepository(context), 
        ITransactionsRepository
    {
        private readonly TransactionMapper mapper = mapper;

        public async Task AddAsync(Transaction transaction, string balanceName)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            var balance = await this.context.Balances
                .AsNoTracking()
                .FirstAsync(balance => balance.Name == balanceName);

            var categoryName = transaction.Category?.Name;
            var category = await this.context.Categories
                .AsNoTracking()
                .FirstAsync(a => a.Name == categoryName);

            var transactionEntity = this.mapper.Map(transaction);

            transactionEntity.CategoryId = category.Id.Value;
            transactionEntity.BalanceId = balance.Id.Value;

            var now = DateTimeOffset.Now;
            transactionEntity.CreationTime = now;
            transactionEntity.UpdateTime = now;

            await this.context.Transactions.AddAsync(transactionEntity);
        }
    }
}
