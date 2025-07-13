using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Domain.Interfaces.Repositories
{
    public interface ITransactionsRepository
    {
        Task CommitAsync();
        Task AddAsync(Transaction transaction, string balanceName);
    }
}
