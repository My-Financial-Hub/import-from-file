using MyFinancialHub.Import.Domain.Entities.Transactions;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class CategoriesRepository(
        FinancialHubContext context, 
        CategoryMapper mapper,
        ILogger<CategoriesRepository> logger
    ) :
        BaseRepository(context, logger), 
        ICategoryRepository
    {
        private readonly CategoryMapper mapper = mapper;

        public async Task AddAsync(Category category)
        {
            using var _ = this.logger.BeginScope("Add Category");
            this.logger.LogDebug("Adding category {CategoryName}", category.Name);

            this.logger.LogTrace("Mapping category {CategoryName} to entity", category.Name);
            var categoryEntity = this.mapper.Map(category);
            this.logger.LogTrace("Category {CategoryName} mapped to entity", category.Name);

            this.logger.LogTrace("Setting timestamps for new category {CategoryName}", category.Name);
            var now = DateTimeOffset.Now;
            categoryEntity.CreationTime = now;
            categoryEntity.UpdateTime = now;
            this.logger.LogTrace("Timestamps set for category {CategoryName}", category.Name);

            this.logger.LogDebug("Adding category {CategoryName}", category.Name);
            await this.context.Categories.AddAsync(categoryEntity);
            this.logger.LogDebug("Category {CategoryName} added", category.Name);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            using var _ = this.logger.BeginScope("Get Category by Name");
            if (string.IsNullOrWhiteSpace(name))
            {
                this.logger.LogWarning("GetByNameAsync called with null or empty name");
                return null;
            }

            this.logger.LogDebug("Retrieving category with name {CategoryName}", name);
            var balanceEntity = await this.context.Categories.FirstOrDefaultAsync(c => c.Name == name);
            if (balanceEntity == null)
            {
                this.logger.LogDebug("Category with name {CategoryName} not found", name);
                return null;
            }
            this.logger.LogDebug("Category with name {CategoryName} retrieved", name);

            this.logger.LogTrace("Mapping category entity to domain model for category {CategoryName}", name);
            var balance = this.mapper.Map(balanceEntity);
            this.logger.LogTrace("Category entity mapped to domain model for category {CategoryName}", name);

            return balance;
        }

        public async Task<IEnumerable<Category>> GetByNamesAsync(string[] names)
        {
            using var _ = this.logger.BeginScope("Get Categories by Names");
            if (names == null || names.Length == 0)
            {
                this.logger.LogWarning("GetByNamesAsync called with null or empty names array");
                return Array.Empty<Category>();
            }

            this.logger.LogDebug("Retrieving categories with names: {CategoryNames}", string.Join(", ", names));
            var categories = await this.context.Categories
                .Where(category => names.Contains(category.Name))
                .Select(category => mapper.Map(category))
                .ToArrayAsync();
            this.logger.LogDebug("Retrieved {Length} categories", categories.Length);

            return categories;
        }
    }
}
