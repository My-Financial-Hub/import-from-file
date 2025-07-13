using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Application.Services
{
    internal class CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
    {
        private readonly ICategoryRepository categoryRepository = categoryRepository;
        private readonly ILogger<CategoryService> logger = logger;

        public async Task InsertIfNotExistsAsync(IEnumerable<Category> categories)
        {
            using var _ = this.logger.BeginScope("InsertIfNotExistsAsync");
            var names = categories.Select(c => c.Name).ToArray();
            try
            {
                var addedAmount = 0;
                var foundCategories = (await this.categoryRepository.GetByNamesAsync(names)).ToDictionary(k => k.Name);
                foreach (var (name, category) in from name in names
                                                 where !foundCategories.ContainsKey(name)
                                                 let category = new Category(name)
                                                 select (name, category))
                {
                    this.logger.LogInformation("Inserting category: {CategoryName}", name);
                    await this.categoryRepository.AddAsync(category);
                    foundCategories[name] = category;
                    addedAmount++;
                }
                if(addedAmount > 0)
                {
                    this.logger.LogInformation("Added {AddedAmount} categories", addedAmount);
                    await this.categoryRepository.CommitAsync();
                }
                else
                {
                    this.logger.LogInformation("No new categories added");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error in InsertIfExistsAsync: {Message}", ex.Message);
                throw;
            }
        }
    }
}
