namespace MyFinancialHub.Pdf.Azure.Mappers
{
    internal class BalanceMapper(TransactionMapper transactionMapper)
    {
        private readonly TransactionMapper transactionMapper = transactionMapper;
        private readonly Dictionary<string, Balance> balances = [];

        private readonly string[] columns =
        [
            "Data",
            "Grupo",
            "Conta",
            "Valor"
        ]; 

        public IEnumerable<Balance> MapFromTransactionTables(IEnumerable<DocumentTable> tables)
        {
            foreach (var rows in from table in tables
                                 let cells = table.Cells
                                 let rows = cells.Skip(columns.Length).ToArray()
                                 select rows)
            {
                for (int i = 0; i < rows.Length / columns.Length; i++)
                {
                    var row = rows.Skip(i * columns.Length).Take(columns.Length).ToArray();
                    var transaction = transactionMapper.Map(row);

                    Balance balance;
                    var balanceName = row[2].Content;
                    if (!balances.TryGetValue(balanceName, out Balance? value))
                    {
                        balance = new Balance(balanceName, 0);
                        balances.Add(balanceName, balance);
                    }
                    else
                    {
                        balance = value;
                    }

                    balance.AddTransaction(transaction);
                }
            }

            return balances.Values;
        }
    }
}
