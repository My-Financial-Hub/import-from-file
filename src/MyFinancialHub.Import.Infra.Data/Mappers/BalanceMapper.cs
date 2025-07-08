namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class BalanceMapper
    {
        public Balance Map(BalanceEntity entity)
        {
            if (entity == null)
                return null;
            var balance = new Balance(entity.Name, 0);
            // balance.AddTransactions(entity.Transactions);

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
