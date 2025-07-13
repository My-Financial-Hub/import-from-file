using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Application.Services
{
    public class BalanceService(
        IBalanceRepository balanceRepository,
        ILogger<BalanceService> logger
    )
    {
        private readonly IBalanceRepository balanceRepository = balanceRepository;
        private readonly ILogger<BalanceService> logger = logger;

        public async Task<IEnumerable<Balance>> InsertAsync(IEnumerable<Balance> balances, string accountName)
        {
            this.logger.BeginScope("Insert Balances");
            foreach (var balance in balances)
            {
                this.logger.LogInformation("Processing balance: {BalanceName}", balance.Name);
                var foundBalance = await this.GetByNameAsync(balance.Name);
                if (foundBalance is null)
                {
                    this.logger.LogInformation("Balance {BalanceName} not found, adding new balance.", balance.Name);
                    await this.AddAsync(balance, accountName);
                }
                else
                {
                    this.logger.LogInformation("Balance {BalanceName} found, updating existing balance.", balance.Name);
                    await this.UpdateAsync(balance, accountName);
                }
            }

            await this.balanceRepository.CommitAsync();
            this.logger.LogInformation("All balances processed successfully.");
            return balances;
        }

        private async Task<Balance?> GetByNameAsync(string name)
        {
            using var _ = this.logger.BeginScope("Get Balance By Name");
            this.logger.LogInformation("Retrieving balance: {Name}", name);
            return await this.balanceRepository.GetByNameAsync(name);
        }

        private async Task<Balance> AddAsync(Balance balance, string accountName)
        {
            using var _ = this.logger.BeginScope("Add Balance");
            if (balance is null)
                throw new ArgumentNullException(nameof(balance), "Balance cannot be null.");
            this.logger.LogInformation("Adding balance: {BalanceName}", balance.Name);
            await this.balanceRepository.AddAsync(balance, accountName);

            return balance;
        }

        private async Task UpdateAsync(Balance balance, string accountName)
        {
            using var _ = this.logger.BeginScope("Update Balance");
            if (balance is null)
                throw new ArgumentNullException(nameof(balance), "Balance cannot be null.");
            this.logger.LogInformation("Updating balance: {BalanceName}", balance.Name);
            await this.balanceRepository.UpdateAsync(balance, accountName);
        }
    }
}
