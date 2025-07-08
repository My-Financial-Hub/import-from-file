namespace MyFinancialHub.Import.Application.Services
{
    internal class AccountService(
        IAccountRepository accountRepository,
        ILogger<AccountService> logger
    )
    {
        private readonly IAccountRepository accountRepository = accountRepository;
        private readonly ILogger<AccountService> logger = logger;

        public async Task<Account> GetOrCreateAsync(string accountName)
        {
            using var _ = this.logger.BeginScope("Get and Create Account By Name");
            this.logger.LogInformation("Retrieving account: {AccountName}", accountName);
            var account =  await this.accountRepository.GetByNameAsync(accountName);

            if (account is null)
            {
                this.logger.LogInformation("Account not found, creating new account: {AccountName}", accountName);
                account = await this.CreateAsync(accountName);
                if (account is null)
                {
                    this.logger.LogError("Failed to create account: {AccountName}", accountName);
                    throw new InvalidOperationException($"Could not create account with name {accountName}");
                }
            }
            else
            {
                this.logger.LogInformation("Found existing account: {AccountName}", account.Name);
            }

            return account;
        }

        public async Task<Account?> GetByNameAsync(string accountName)
        {
            using var _ = this.logger.BeginScope("Get Account By Name");
            this.logger.LogInformation("Retrieving account: {AccountName}", accountName);
            return await this.accountRepository.GetByNameAsync(accountName);
        }

        public async Task<Account?> CreateAsync(string accountName)
        {
            using var _ = this.logger.BeginScope("Create Account By Name");
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name cannot be null or empty.", nameof(accountName));
            
            var account = new Account(accountName, 0);
            this.logger.LogInformation("Retrieving account: {AccountName}", accountName);
            await this.accountRepository.CreateAsync(account);
            this.logger.LogInformation("Account {AccountName} created", accountName);

            return account;
        }

        public async Task UpdateAsync(Account account)
        {
            using var _ = this.logger.BeginScope("Update Account");
            if (account is null)
                throw new ArgumentNullException(nameof(account), "Account cannot be null.");

            this.logger.LogInformation("Updating account: {AccountName}", account.Name);
            await this.accountRepository.UpdateAsync(account);
        } 
    }
}
