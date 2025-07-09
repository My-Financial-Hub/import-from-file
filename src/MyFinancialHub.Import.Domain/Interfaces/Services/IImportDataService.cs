namespace MyFinancialHub.Import.Domain.Interfaces.Services
{
    public interface IImportDataService
    {
        Task<ImportData> ImportAsync(Stream fileStream);
    }
}
