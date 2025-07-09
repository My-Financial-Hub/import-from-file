namespace MyFinancialHub.Import.Domain.Entities.Transactions
{
    public class Transaction(string description, decimal amount, DateTime date)
    {
        public Category? Category { get; set; }
        public string Description { get; set; } =  description?? string.Empty;
        public decimal Amount { get; set; } = amount;
        public DateTime Date { get; set; } = date;

        public TransactionType TransactionType { get; set; } = amount > 0 ? TransactionType.Earn : TransactionType.Expense;
        public TransactionStatus Status { get; set; } = TransactionStatus.Committed;
    }
}
