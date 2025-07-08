namespace MyFinancialHub.Import.Domain.Entities
{
    public class Balance(string name, decimal amount)
    {
        public string Name { get; set; } = name;
        public decimal Amount { get; set; } = amount;
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
            Transactions.Add(transaction);
            //TODO: Adjust the amount based on the transaction type
            Amount += transaction.Amount;
        }

        public void AddTransactions(ICollection<Transaction> transactions)
        {
            throw new NotImplementedException();
        }
    }
}
