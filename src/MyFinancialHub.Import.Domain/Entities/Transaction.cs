namespace MyFinancialHub.Import.Domain.Entities
{
    public class Transaction
    {
        public Category Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public TransactionType TransactionType { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Committed;
    }
}
