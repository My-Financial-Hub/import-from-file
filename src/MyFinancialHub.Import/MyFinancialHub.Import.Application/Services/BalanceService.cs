using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Application.Services
{
    internal class BalanceService(
        IBalanceRepository balanceRepository,
        ILogger<BalanceService> logger
    )
    {
        private readonly IBalanceRepository balanceRepository = balanceRepository;
        private readonly ILogger<BalanceService> logger = logger;

        public async Task<IEnumerable<Balance>> InsertAsync(IEnumerable<Balance> balances, string accountName)
        {
            using var _ = this.logger.BeginScope("Insert Balances");
            var names = balances
                .Select(c => c.Name)
                .Distinct()
                .ToArray();
            this.logger.LogInformation("Processing {Count} balances for account {AccountName}", names.Length, accountName);

            try
            {
                var balanceResult = new List<Balance>();
                var foundBalances = await this.GetBalancesDictionaryAsync(names);
                
                this.logger.LogInformation("Inserting/Updating balances");
                foreach (var balanceName in names)
                {
                    this.logger.LogTrace("Processing balance: {BalanceName}", balanceName);
                    var newBalance = balances.First(b => b.Name == balanceName);

                    if(foundBalances.ContainsKey(balanceName))
                    {
                        this.logger.LogTrace("Balance {BalanceName} exists, updating", balanceName);
                        await this.balanceRepository.UpdateAsync(newBalance, accountName);
                    }
                    else
                    {
                        this.logger.LogTrace("Balance {BalanceName} does not exist, adding", balanceName);
                        await this.balanceRepository.AddAsync(newBalance, accountName);
                    }

                    this.logger.LogTrace("Balance {BalanceName} processed", balanceName);
                    balanceResult.Add(newBalance);
                }

                this.logger.LogTrace("Committing balance changes to the repository");
                await this.balanceRepository.CommitAsync();
                this.logger.LogInformation("Inserted/Updated {Count} balances", balanceResult.Count);

                return balanceResult;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error in Balance InsertAsync: {Message}", ex.Message);
                throw;
            }
        }

        private async Task<Dictionary<string, Balance>> GetBalancesDictionaryAsync(string[] names)
        {
            using var _ = this.logger.BeginScope("Get Balances Dictionary");

            this.logger.LogTrace("Retrieving balances for names: {Names}", string.Join(", ", names));
            var balances = await this.balanceRepository.GetByNamesAsync(names);
            this.logger.LogTrace("Retrieved {Count} balances", balances.Count());
            
            return balances.ToDictionary(k => k.Name);
        }
    }
}
