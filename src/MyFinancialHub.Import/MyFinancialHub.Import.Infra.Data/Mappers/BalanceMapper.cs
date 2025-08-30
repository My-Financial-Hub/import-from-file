using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class BalanceMapper
    {
        public Balance Map(BalanceEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new Balance(entity.Name, entity.Amount);
        }

        public BalanceEntity Map(Balance balance, Guid accountId)
        {
            ArgumentNullException.ThrowIfNull(balance);

            var entity = new BalanceEntity
            {
                Name = balance.Name,
                Amount = balance.Amount,
                AccountId = accountId,
                Currency = string.Empty,
                Transactions = new List<TransactionEntity>(),
                IsActive = true, 
            };
            return entity;
        }
    }
}
