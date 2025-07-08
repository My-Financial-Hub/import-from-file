namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Entities
{
    internal record class PdfDataAggregate(
        List<PdfCategory> Categories,
        List<PdfBalance> Balances,
        List<PdfTransaction> Transactions
    );
}
