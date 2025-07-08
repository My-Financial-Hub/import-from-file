using System.Globalization;

namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Pdf
{
    internal class PdfTransactionMapper
    {
        private readonly string[] columns =
        [
            "Data",
            "Grupo",
            "Conta",
            "Valor"
        ];

        public PdfTransaction Map(DocumentTableCell[] cells)
        {
            if (cells == null || cells.Length != columns.Length)
                throw new ArgumentException($"Invalid cells array. It must contain {columns.Length} cells.", nameof(cells));

            return new PdfTransaction(
                DateTime.ParseExact(cells[0]?.Content, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                cells[1].Content,
                cells[2].Content,
                decimal.Parse(cells[3]?.Content ?? "0", NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"))
            );
        }

        public IEnumerable<PdfTransaction> Map(IEnumerable<DocumentTable> tables)
        {
            var transactions = new List<PdfTransaction>();
            foreach (var rows in from table in tables
                                 let cells = table.Cells
                                 let rows = cells.Skip(columns.Length).ToArray()
                                 select rows)
            {
                for (int i = 0; i < rows.Length / columns.Length; i++)
                {
                    var row = rows.Skip(i * columns.Length).Take(columns.Length).ToArray();
                    var transaction = Map(row);

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
    }
}