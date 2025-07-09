using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class CategoriesRepository(FinancialHubContext context) : ICategoryRepository
    {
        private readonly FinancialHubContext context = context;

        public Task AddAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetByNamesAsync(string[] names)
        {
            throw new NotImplementedException();
        }
    }
}
