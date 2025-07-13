using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class AccountsRepository(FinancialHubContext context, AccountMapper mapper) :
        BaseRepository(context), 
        IAccountRepository
    {
        private readonly AccountMapper mapper = mapper;

        public async Task CreateAsync(Account account)
        {
            var entity = this.mapper.Map(account);

            var now = DateTimeOffset.Now;
            entity.CreationTime = now;
            entity.UpdateTime = now;

            await this.context.Accounts.AddAsync(entity);
        }

        public async Task<Account> GetByNameAsync(string name)
        {
            var account = await this.context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Name == name);
            if (account == null)
                return null;

            return this.mapper.Map(account);
        }

        public async Task UpdateAsync(Account account)
        {
            var foundAccount = await this.context.Accounts
                .AsNoTracking()
                .FirstAsync(b => b.Name == account.Name);

            var entity = this.mapper.Map(account);
            entity.Id = foundAccount.Id;
            entity.CreationTime = foundAccount.CreationTime;
            entity.UpdateTime = DateTimeOffset.Now;

            this.context.Accounts.Update(entity);
        }
    }
}
