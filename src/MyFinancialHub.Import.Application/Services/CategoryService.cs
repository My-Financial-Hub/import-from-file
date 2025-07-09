using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Application.Services
{
    internal class CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
    {
        private readonly ICategoryRepository categoryRepository = categoryRepository;
        private readonly ILogger<CategoryService> logger = logger;

        public async Task<IEnumerable<Category>> GetByNamesAsync(string[] names)
        {
            try
            {
                var categories = (await this.categoryRepository.GetByNamesAsync(names)).ToDictionary(k => k.Name);
                foreach (var (name, category) in from name in names
                                                 where !categories.ContainsKey(name)
                                                 let category = new Category(name)
                                                 select (name, category))
                {
                    await this.categoryRepository.AddAsync(category);
                    categories[name] = category;
                }

                return categories.Values;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving category by names: {Message}", ex.Message);
                throw;
            }
        }
    }
}
