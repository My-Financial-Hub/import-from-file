using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Domain.Interfaces.Repositories
{
    public interface IBalanceRepository
    {
        Task<Balance?> GetByNameAsync(string name);

        Task AddAsync(Balance account);

        Task UpdateAsync(Balance account);
    }
}
