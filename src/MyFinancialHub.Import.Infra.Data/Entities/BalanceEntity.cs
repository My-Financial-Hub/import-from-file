namespace MyFinancialHub.Import.Infra.Data.Entities
{
    public class BalanceEntity : BaseEntity
    {
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public required decimal Amount { get; set; }
        public required bool IsActive { get; set; }

        public Guid AccountId { get; set; }
        public AccountEntity? Account { get; set; }

        public ICollection<TransactionEntity>? Transactions { get; set; }
    }
}
