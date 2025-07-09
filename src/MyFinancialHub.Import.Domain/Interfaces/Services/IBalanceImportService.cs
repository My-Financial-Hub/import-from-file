using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Domain.Interfaces.Services
{
    public interface IBalanceImportService
    {
        Task<IEnumerable<Balance>> ImportAsync(Stream fileStream);
    }
}
