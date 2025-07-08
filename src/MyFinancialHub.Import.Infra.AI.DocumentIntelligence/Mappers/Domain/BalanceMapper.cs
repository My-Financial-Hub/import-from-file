namespace MyFinancialHub.Import.Infra.AI.DocumentIntelligence.Mappers.Domain
{
    internal class BalanceMapper(TransactionMapper transactionMapper)
    {
        private readonly TransactionMapper transactionMapper = transactionMapper;

        public IEnumerable<Balance> Map(PdfDataAggregate dataAggregate)
        {
            var balances = new Dictionary<string, Balance>();

            foreach (var transaction in dataAggregate.Transactions)
            {
                if (transaction is null)
                    continue;

                var createdTransaction = new Transaction()
                {
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Description = string.Empty,
                    Status = TransactionStatus.Committed,
                    //TransactionType = TransactionType.Earn, Check how to set this properly
                    //The new document structure now have a TransactionType
                    Category = new Category(transaction.Category),
                };

                if (!balances.TryGetValue(transaction.Balance, out Balance? balance))
                {
                    balance = new Balance(transaction.Balance, 0);
                    balances.Add(transaction.Balance, balance);
                }

                balance.AddTransaction(createdTransaction);
            }

            return balances.Values;
        }
    }
}
