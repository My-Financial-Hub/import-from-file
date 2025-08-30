using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Domain.Interfaces.Repositories
{
    public interface ITransactionsRepository
    {
        Task CommitAsync();

        Task AddAsync(string balanceName, Transaction transaction);

        Task AddAsync(string balanceName, IEnumerable<Transaction> transactions);
    }
}
