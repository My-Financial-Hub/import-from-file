using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Application.Services
{
    internal class CategoryService(
        ICategoryRepository categoryRepository, 
        ILogger<CategoryService> logger
    )
    {
        private readonly ICategoryRepository categoryRepository = categoryRepository;
        private readonly ILogger<CategoryService> logger = logger;

        public async Task InsertIfNotExistsAsync(IEnumerable<Category> categories)
        {
            using var _ = this.logger.BeginScope("InsertIfNotExistsAsync");
            var names = categories
                .Select(c => c.Name)
                .Distinct()
                .ToArray();
            this.logger.LogInformation("Processing {Count} categories", names.Length);

            try
            {
                var addedAmount = 0;
                var foundCategories = await this.GetCategoriesDicionaryAsync(names);
                this.logger.LogInformation("Inserting missing categories");
                foreach (
                    var (name, category) in 
                        from name in names where !foundCategories.ContainsKey(name)
                        let category = new Category(name)
                        select (name, category)                        
                )
                {
                    this.logger.LogDebug("Inserting category: {CategoryName}", name);
                    await this.categoryRepository.AddAsync(category);
                    this.logger.LogDebug("Category {CategoryName} inserted", name);
                    foundCategories[name] = category;
                    addedAmount++;
                }

                if(addedAmount > 0)
                {
                    this.logger.LogInformation("Committing {Count} new categories to the repository", addedAmount);
                    await this.categoryRepository.CommitAsync();
                    this.logger.LogInformation("Inserted {Count} new categories", addedAmount);
                }
                else
                {
                    this.logger.LogInformation("No new categories to insert");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error in InsertIfExistsAsync: {Message}", ex.Message);
                throw;
            }
        }

        private async Task<Dictionary<string, Category>> GetCategoriesDicionaryAsync(string[] names)
        {
            using var _ = this.logger.BeginScope("GetCategoriesDicionaryAsync");
            this.logger.LogDebug("Retrieving categories for names: {Names}", string.Join(", ", names));
            var categories = await this.categoryRepository.GetByNamesAsync(names);
            this.logger.LogDebug("Retrieved {Count} categories", categories.Count());
            return categories.ToDictionary(k => k.Name);
        }
    }
}
