using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class BalanceMapper
    {
        public Balance Map(BalanceEntity entity)
        {
            if (entity == null)
                return null;
            var balance = new Balance(entity.Name, entity.Amount);
            return balance;
        }

        public BalanceEntity Map(Balance balance)
        {
            if (balance == null)
                return null;

            var entity = new BalanceEntity
            {
                Name = balance.Name,
            };
            return entity;
        }
    }
}
