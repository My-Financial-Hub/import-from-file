using MyFinancialHub.Import.Domain.Entities.Accounts;
using MyFinancialHub.Import.Infra.Data.Mappers;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class AccountsRepository(FinancialHubContext context, AccountMapper mapper) : IAccountRepository
    {
        private readonly FinancialHubContext context = context;
        private readonly AccountMapper mapper = mapper;

        public async Task CreateAsync(Account account)
        {
            var entity = this.mapper.Map(account);

            var now = DateTime.UtcNow;
            entity.CreationTime = now;
            entity.UpdateTime = now;

            await this.context.Accounts.AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<Account> GetByNameAsync(string name)
        {
            var account = await this.context.Accounts.FirstOrDefaultAsync(a => a.Name == name);
            if (account == null)
                return null;

            return this.mapper.Map(account);
        }

        public async Task UpdateAsync(Account account)
        {
            var entity = this.mapper.Map(account);

            this.context.Accounts.Update(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
