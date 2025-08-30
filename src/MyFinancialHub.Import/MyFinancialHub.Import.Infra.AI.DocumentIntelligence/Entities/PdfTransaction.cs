namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Entities
{
    internal record class PdfTransaction(
        DateTime Date,
        string Category, 
        string Balance, 
        string Description,
        decimal Amount
    );
}
