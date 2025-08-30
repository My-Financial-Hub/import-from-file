using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class AccountsRepository(
        FinancialHubContext context, 
        AccountMapper mapper,
        ILogger<AccountsRepository> logger
    ) 
        : BaseRepository(context, logger), IAccountRepository
    {
        private readonly AccountMapper mapper = mapper;

        public async Task CreateAsync(Account account)
        {
            this.logger.BeginScope("Creating account");
            if (account is null)
            {
                var ex = new ArgumentNullException(nameof(account), "Account cannot be null.");
                this.logger.LogError(ex, "Failed to Account");
                throw ex;
            }
            this.logger.LogTrace("Creating new account {AccountName}", account.Name);
            var entity = this.mapper.Map(account);
            this.logger.LogTrace("Mapping completed for account {AccountName}", account.Name);

            this.logger.LogTrace("Setting timestamps for new account {AccountName}", account.Name);
            var now = DateTimeOffset.Now;
            entity.CreationTime = now;
            entity.UpdateTime = now;
            this.logger.LogTrace("Timestamps set for account {AccountName}", account.Name);

            this.logger.LogDebug("Adding Account {AccountName}", account.Name);
            await this.context.Accounts.AddAsync(entity);
            this.logger.LogDebug("Account {AccountName} added", account.Name);
        }

        public async Task<Account?> GetByNameAsync(string name)
        {
            this.logger.BeginScope("Get Account by Name");
            if (string.IsNullOrWhiteSpace(name))
            {
                this.logger.LogWarning("GetByNameAsync called with null or empty name");
                return null;
            }

            this.logger.LogDebug("Retrieving account with name {AccountName}", name);
            var account = await this.context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Name == name);

            if (account == null) {
                this.logger.LogInformation("No account found with name {AccountName}", name);
                return null;
            }
            this.logger.LogDebug("Account {AccountName} retrieved successfully", name);

            this.logger.LogTrace("Mapping AccountEntity to Account for account {AccountName}", name);
            var result = this.mapper.Map(account);
            this.logger.LogTrace("Mapping completed for account {AccountName}", name);

            return result;
        }
    }
}
