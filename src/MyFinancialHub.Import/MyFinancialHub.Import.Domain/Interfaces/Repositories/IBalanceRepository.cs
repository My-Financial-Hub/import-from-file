using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Domain.Interfaces.Repositories
{
    public interface IBalanceRepository
    {
        Task<Balance?> GetByNameAsync(string name);

        Task<IEnumerable<Balance>> GetByNamesAsync(params string[] names);

        Task AddAsync(Balance balance, string accountName);

        Task UpdateAsync(Balance balance, string accountName);

        Task CommitAsync();
    }
}
