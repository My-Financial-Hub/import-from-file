using System.Globalization;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Domain
{
    internal class TransactionMapper
    {
        public Transaction Map(DocumentTableCell[] cells)
        {
            if (cells == null || cells.Length < 4)
                throw new ArgumentException("Invalid cells array. It must contain at least 4 cells.", nameof(cells));

            var date = DateTime.ParseExact(cells[0]?.Content, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var amount = decimal.Parse(cells[3]?.Content ?? "0", NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"));
            return new Transaction
            {
                Amount = amount,
                Date = date,
                Description = string.Empty,
                Category = new Category(cells[1].Content),
            };
        }
    }
}
