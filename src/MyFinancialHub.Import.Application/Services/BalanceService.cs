namespace MyFinancialHub.Import.Application.Services
{
    public class BalanceService(
        IBalanceRepository balanceRepository,
        ILogger<BalanceService> logger
    )
    {
        private readonly IBalanceRepository balanceRepository = balanceRepository;
        private readonly ILogger<BalanceService> logger = logger;

        public async Task<IEnumerable<Balance>> InsertAsync(IEnumerable<Balance> balances)
        {
            foreach (var balance in balances)
            {
                var foundBalance = await this.GetByNameAsync(balance.Name);
                if (foundBalance is null)
                {
                    await this.AddAsync(balance);
                }
                else
                {
                    await this.UpdateAsync(balance);
                }
            }
            return balances;
        }

        public async Task<Balance?> GetByNameAsync(string name)
        {
            using var _ = this.logger.BeginScope("Get Balance By Name");
            this.logger.LogInformation("Retrieving balance: {Name}", name);
            return await this.balanceRepository.GetByNameAsync(name);
        }

        public async Task<Balance> AddAsync(Balance balance)
        {
            using var _ = this.logger.BeginScope("Add Balance");
            if (balance is null)
                throw new ArgumentNullException(nameof(balance), "Balance cannot be null.");
            this.logger.LogInformation("Adding balance: {BalanceName}", balance.Name);
            await this.balanceRepository.AddAsync(balance);

            return balance;
        }

        public async Task UpdateAsync(Balance balance)
        {
            using var _ = this.logger.BeginScope("Update Balance");
            if (balance is null)
                throw new ArgumentNullException(nameof(balance), "Balance cannot be null.");
            this.logger.LogInformation("Updating balance: {BalanceName}", balance.Name);
            await this.balanceRepository.UpdateAsync(balance);
        }
    }
}
