using MyFinancialHub.Import.Domain.Entities.Transactions;
using System.Linq;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class CategoriesRepository(FinancialHubContext context, CategoryMapper mapper) :
        BaseRepository(context), 
        ICategoryRepository
    {
        private readonly CategoryMapper mapper = mapper;

        public async Task AddAsync(Category category)
        {
            var categoryEntity = this.mapper.Map(category);
            var now = DateTimeOffset.Now;
            categoryEntity.CreationTime = now;
            categoryEntity.UpdateTime = now;

            await this.context.Categories.AddAsync(categoryEntity);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await this.context.Categories
                .FirstOrDefaultAsync(c => c.Name == name)
                .ContinueWith(task => task.Result is null ? null : this.mapper.Map(task.Result));
        }

        public async Task<IEnumerable<Category>> GetByNamesAsync(string[] names)
        {
            return await this.context.Categories
                .Where(category => names.Contains(category.Name))
                .Select(category => mapper.Map(category))
                .ToListAsync();
        }
    }
}
