using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetByNamesAsync(string[] names);
        Task AddAsync(Category category);
        Task CommitAsync();
    }
}
