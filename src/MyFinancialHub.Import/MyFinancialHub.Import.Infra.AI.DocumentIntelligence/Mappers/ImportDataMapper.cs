using MyFinancialHub.Import.Domain.Entities.Accounts;
using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers
{
    internal class ImportDataMapper
    {
        public ImportData Map(PdfImportData pdfData)
        {
            var categories = pdfData.Categories.Select(c => new Category(c.Name)).ToDictionary(k => k.Name);
            var balances = pdfData.Balances.Select(b => new Balance(b.Name, b.Amount)).ToDictionary(k => k.Name);
            var transactions = new List<Transaction>();
            foreach (var pdfTransaction in pdfData.Transactions)
            {
                if(balances.TryGetValue(pdfTransaction.Balance, out var balance))
                {
                    var transaction = new Transaction(
                        pdfTransaction.Description, 
                        pdfTransaction.Amount, 
                        pdfTransaction.Date,
                        categories.GetValueOrDefault(pdfTransaction.Category)
                    );
                    balance.AddTransaction(transaction);
                    transactions.Add(transaction);
                }
                else
                {
                    throw new InvalidDataException($"Balance '{pdfTransaction.Balance}' not found for transaction on {pdfTransaction.Date}.");
                }
            }
            return new ImportData(
                categories.Values.ToList(), 
                balances.Values.ToList(), 
                transactions
            );
        }
    }
}
