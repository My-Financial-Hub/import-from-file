using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Application.Services
{
    internal class AccountService(
        IAccountRepository accountRepository,
        ILogger<AccountService> logger
    )
    {
        private readonly IAccountRepository accountRepository = accountRepository;
        private readonly ILogger<AccountService> logger = logger;

        public async Task<Account?> GetByNameAsync(string accountName)
        {
            using var _ = this.logger.BeginScope("Get Account By Name");

            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name cannot be null or empty.", nameof(accountName));
            
            this.logger.LogInformation("Retrieving account: {AccountName}", accountName);
            var account = await this.accountRepository.GetByNameAsync(accountName);
            this.logger.LogInformation("Account retrieval completed for: {AccountName}", accountName);
           
            return account;
        }

        public async Task<Account> CreateAsync(string accountName)
        {
            using var _ = this.logger.BeginScope("Create Account");
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name cannot be null or empty.", nameof(accountName));
            
            var account = new Account(accountName, 0);
            this.logger.LogInformation("Creating account: {AccountName}", accountName);
            await this.accountRepository.CreateAsync(account);
            this.logger.LogInformation("Account {AccountName} created", accountName);
            return account;
        }
    }
}
