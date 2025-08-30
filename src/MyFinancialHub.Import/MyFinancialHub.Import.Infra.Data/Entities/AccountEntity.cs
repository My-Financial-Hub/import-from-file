namespace MyFinancialHub.Import.Infra.Data.Entities
{
    public class AccountEntity : BaseEntity
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public required bool IsActive { get; init; }

        public ICollection<BalanceEntity> Balances { get; init; } = new List<BalanceEntity>();
    }
}
