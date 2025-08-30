using MyFinancialHub.Import.Domain.Entities.Accounts;
using System.Linq;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class AccountMapper(BalanceMapper balanceMapper)
    {
        private readonly BalanceMapper balanceMapper = balanceMapper;

        public Account Map(AccountEntity entity)
        {            
            var account = new Account(entity.Name, 0);

            var balances = entity.Balances;
            if (balances is not null && balances.Count > 0) 
            {
                account.AddBalances(
                    balances.Select(balanceMapper.Map)
                );
            }

            return account;
        }

        public AccountEntity Map(Account account)
        {
            var entity = new AccountEntity
            {
                Name = account.Name,
                IsActive = true
            };
            return entity;
        }
    }
}
