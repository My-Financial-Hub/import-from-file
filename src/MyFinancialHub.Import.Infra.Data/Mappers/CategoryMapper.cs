using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class CategoryMapper
    {
        public Category Map(CategoryEntity category)
        {
            return new Category(category.Name);
        }

        public CategoryEntity Map(Category category)
        {
            return new CategoryEntity()
            {
                Name = category.Name,
                Description = string.Empty,
                IsActive = true,
            };
        }
    }
}
