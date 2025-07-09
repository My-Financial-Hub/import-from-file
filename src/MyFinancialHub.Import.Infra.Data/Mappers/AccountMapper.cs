using MyFinancialHub.Import.Domain.Entities.Accounts;
using System.Linq;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class AccountMapper(BalanceMapper balanceMapper)
    {
        private readonly BalanceMapper balanceMapper = balanceMapper;

        public Account Map(AccountEntity entity)
        {
            if (entity == null)
                return null;
            var account = new Account(entity.Name, 0);
            if(entity.Balances?.Count > 0)
            {
                account.AddBalances(entity.Balances.Select(balanceMapper.Map));
            }

            return account;
        }

        public AccountEntity Map(Account account)
        {
            if (account == null)
                return null;

            var entity = new AccountEntity
            {
                Name = account.Name,
                Balances = account.Balances.Select(balanceMapper.Map).ToArray(),
            };
            return entity;
        }
    }
}
