using MyFinancialHub.Import.Domain.Entities;
using MyFinancialHub.Import.Domain.Entities.Accounts;

namespace MyFinancialHub.Import.Application.Services
{
    internal class ImportDataService(
        CategoryService categoryService,
        BalanceService balanceService,
        TransactionService transactionService,
        ILogger<ImportDataService> logger
    )
    {
        private readonly CategoryService categoryService = categoryService;
        private readonly BalanceService balanceService = balanceService;
        private readonly TransactionService transactionService = transactionService;
        private readonly ILogger<ImportDataService> logger = logger;

        public async Task ImportAsync(ImportData importData, Account account)
        {
            using var _ = this.logger.BeginScope("ImportDataService");
            this.logger.LogInformation("Starting import for account: {AccountName}", account.Name);

            this.logger.LogInformation("Inserting categories");
            await this.categoryService.InsertIfNotExistsAsync(importData.Categories);
            this.logger.LogInformation("Categories inserted/updated successfully");

            this.logger.LogInformation("Inserting balances for account: {AccountName}", account.Name);
            var balances = await this.balanceService.InsertAsync(importData.Balances, account.Name);
            this.logger.LogInformation("Balances for account {AccountName} inserted/updated successfully", account.Name);

            this.logger.LogInformation("Inserting transactions for account: {AccountName}", account.Name);
            foreach (
                var (balance, transactions) in from balance in balances
                                               where balance.Transactions.Count > 0
                                               let transactions = balance.Transactions
                                               select (balance, transactions)
            )
            {
                this.logger.LogInformation("Inserting transactions for balance: {BalanceName}", balance.Name);
                await this.transactionService.InsertAsync(transactions, balance.Name);
                this.logger.LogInformation("Transactions for balance {BalanceName} inserted successfully", balance.Name);
            }
            this.logger.LogInformation("All transactions inserted successfully for account: {AccountName}", account.Name);

            this.logger.LogInformation("Import completed for account: {AccountName}", account.Name);
        }
    }
}
