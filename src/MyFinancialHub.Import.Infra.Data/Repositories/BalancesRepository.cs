using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class BalancesRepository(FinancialHubContext context, BalanceMapper mapper) :
        BaseRepository(context), 
        IBalanceRepository
    {
        private readonly BalanceMapper mapper = mapper;

        public async Task AddAsync(Balance balance, string accountName)
        {
            if (balance == null) 
                throw new ArgumentNullException(nameof(balance), "Balance cannot be null.");
            var account = await this.context.Accounts.FirstAsync(a => a.Name == accountName);

            var balanceEntity = this.mapper.Map(balance, account.Id.Value);
            var now = DateTimeOffset.Now;
            balanceEntity.CreationTime = now;
            balanceEntity.UpdateTime = now;

            await this.context.Balances.AddAsync(balanceEntity);
        }

        public async Task<Balance> GetByNameAsync(string name)
        {
            return await this.context.Balances
                .AsNoTracking()
                .FirstOrDefaultAsync(balance => balance.Name == name)
                .ContinueWith(task => task.Result is null ? null : this.mapper.Map(task.Result));
        }

        public async Task UpdateAsync(Balance balance, string accountName)
        {
            if (balance == null)
                throw new ArgumentNullException(nameof(balance), "Balance cannot be null.");
            var account = await this.context.Accounts
                .AsNoTracking()
                .FirstAsync(a => a.Name == accountName);
            var foundBalance = await this.context.Balances
                .AsNoTracking()
                .FirstAsync(b => b.Name == balance.Name && b.AccountId == account.Id.Value);

            var balanceEntity = this.mapper.Map(balance, account.Id.Value);
            balanceEntity.Id = foundBalance.Id;
            balanceEntity.AccountId = foundBalance.AccountId;
            balanceEntity.CreationTime = foundBalance.CreationTime;
            balanceEntity.UpdateTime = DateTimeOffset.Now;

            this.context.Balances.Update(balanceEntity);
        }
    }
}
