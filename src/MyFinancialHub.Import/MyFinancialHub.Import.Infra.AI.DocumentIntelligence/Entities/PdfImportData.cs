namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Entities
{
    internal record class PdfImportData(
        List<PdfCategory> Categories,
        List<PdfBalance> Balances,
        List<PdfTransaction> Transactions
    );
}
