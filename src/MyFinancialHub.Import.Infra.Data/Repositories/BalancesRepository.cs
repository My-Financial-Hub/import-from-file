using MyFinancialHub.Import.Domain.Entities.Accounts;
using System.Linq;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class BalancesRepository(
        FinancialHubContext context, 
        BalanceMapper mapper, 
        ILogger<BalancesRepository> logger
    ) : BaseRepository(context, logger), IBalanceRepository
    {
        private readonly BalanceMapper mapper = mapper;

        public async Task AddAsync(Balance balance, string accountName)
        {
            using var _ = this.logger.BeginScope("Add Balance to Account");
            this.logger.LogDebug("Adding new balance to account {AccountName}", accountName);
            
            if (balance is null)
            {
                var ex = new ArgumentNullException(nameof(balance), "Balance cannot be null.");
                this.logger.LogError(ex, "Failed to add balance to account {AccountName}", accountName);
                throw ex;
            }

            this.logger.LogDebug("Retrieving account {AccountName} for balance addition", accountName);
            var account = await this.context.Accounts.FirstAsync(a => a.Name == accountName);
            this.logger.LogDebug("Account {AccountName} retrieved successfully", accountName);

            this.logger.LogTrace("Mapping Balance to BalanceEntity for account {AccountName}", accountName);
            var balanceEntity = this.mapper.Map(balance, account.Id.GetValueOrDefault());
            this.logger.LogTrace("Mapping completed for balance {BalanceName} in account {AccountName}", balance.Name, accountName);

            this.logger.LogTrace("Setting timestamps for new balance {BalanceName} in account {AccountName}", balance.Name, accountName);
            var now = DateTimeOffset.Now;
            balanceEntity.CreationTime = now;
            balanceEntity.UpdateTime = now;
            this.logger.LogTrace("Timestamps set for balance {BalanceName} in account {AccountName}", balance.Name, accountName);

            this.logger.LogDebug("Adding Balance {BalanceName} to context for account {AccountName}", balance.Name, accountName);
            await this.context.Balances.AddAsync(balanceEntity);
            this.logger.LogDebug("Balance {BalanceName} added to context for account {AccountName}", balance.Name, accountName);
        }

        public async Task<Balance?> GetByNameAsync(string name)
        {
            using var _ = this.logger.BeginScope("Get Balance by Name");
            if (string.IsNullOrWhiteSpace(name))
            {
                this.logger.LogWarning("GetByNameAsync called with null or empty name");
                return null;
            }

            this.logger.LogDebug("Retrieving balance with name {BalanceName}", name);
            var balance = await this.context.Balances
                .AsNoTracking()
                .FirstOrDefaultAsync(balance => balance.Name == name)
                .ContinueWith(task => task.Result is null ? null : this.mapper.Map(task.Result));

            if (balance is null)
            {
                this.logger.LogDebug("No balance found with name {BalanceName}", name);
                return null;
            }

            this.logger.LogDebug("Balance {BalanceName} retrieved successfully", name);
            return balance;
        }

        public async Task<IEnumerable<Balance>> GetByNamesAsync(params string[] names)
        {
            using var _ = this.logger.BeginScope("Get Balances by Names");
            if (names == null || names.Length == 0)
            {
                this.logger.LogWarning("GetByNamesAsync called with null or empty names array");
                return Array.Empty<Balance>();
            }

            this.logger.LogDebug("Retrieving {Count} balances for names: {Names}", names.Length, string.Join(", ", names));
            var balances = await this.context.Balances
                .AsNoTracking()
                .Where(balance => names.Contains(balance.Name))
                .Select(balance => this.mapper.Map(balance))
                .ToArrayAsync();
            this.logger.LogDebug("Retrieved {Count} balances for provided names", balances.Length);

            return balances;
        }

        public async Task UpdateAsync(Balance balance, string accountName)
        {
            using var _ = this.logger.BeginScope("Update Balance in Account");
            this.logger.LogDebug("Updating balance in account {AccountName}", accountName);
            if (balance is null)
            {
                var ex = new ArgumentNullException(nameof(balance), "Balance cannot be null.");
                this.logger.LogError(ex, "Failed to update balance in account {AccountName}", accountName);
                throw ex;
            }

            this.logger.LogDebug("Retrieving account {AccountName} for balance update", accountName);
            var account = await this.context.Accounts
                .AsNoTracking()
                .FirstAsync(a => a.Name == accountName);
            this.logger.LogDebug("Account {AccountName} retrieved successfully", accountName);

            this.logger.LogDebug("Retrieving existing balance {BalanceName} for update in account {AccountName}", balance.Name, accountName);
            var foundBalance = await this.context.Balances
                .AsNoTracking()
                .FirstAsync(b => b.Name == balance.Name && b.AccountId == account.Id);
            this.logger.LogDebug("Existing balance {BalanceName} retrieved successfully for update in account {AccountName}", balance.Name, accountName);

            this.logger.LogTrace("Mapping updated Balance to BalanceEntity for account {AccountName}", accountName);
            var balanceEntity = this.mapper.Map(balance, account.Id.GetValueOrDefault());
            this.logger.LogTrace("Mapping completed for balance {BalanceName} in account {AccountName}", balance.Name, accountName);

            this.logger.LogTrace("Setting identifiers and timestamps for balance {BalanceName} update in account {AccountName}", balance.Name, accountName);
            balanceEntity.Id = foundBalance.Id;
            balanceEntity.AccountId = foundBalance.AccountId;
            balanceEntity.CreationTime = foundBalance.CreationTime;
            balanceEntity.UpdateTime = DateTimeOffset.Now;
            this.logger.LogTrace("Identifiers and timestamps set for balance {BalanceName} in account {AccountName}", balance.Name, accountName);

            this.logger.LogDebug("Updating Balance {BalanceName} in context for account {AccountName}", balance.Name, accountName);
            this.context.Balances.Update(balanceEntity);
            this.logger.LogDebug("Balance {BalanceName} updated in context for account {AccountName}", balance.Name, accountName);
        }
    }
}
