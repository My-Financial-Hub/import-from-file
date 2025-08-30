namespace MyFinancialHub.Import.Domain.Entities
{
    public record class ImportDataEvent(string FilePath, string AccountName);
}
