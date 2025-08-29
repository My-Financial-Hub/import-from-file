using MyFinancialHub.Import.Application.Services;

namespace MyFinancialHub.Import.Application.Handlers.ImportPdfFile
{
    internal class ImportPdfFileCommandHandler(
        AccountService accountService,
        BalanceService balanceService,
        CategoryService categoryService,
        TransactionService transactionService,
        IImportDataService importService,
        ILogger<ImportPdfFileCommandHandler> logger
    ) : ICommandHandler<ImportPdfFileCommand>
    {
        private readonly AccountService accountService = accountService;
        private readonly BalanceService balanceService = balanceService;
        private readonly CategoryService categoryService = categoryService;
        private readonly TransactionService transactionService = transactionService;
        private readonly IImportDataService importService = importService;
        private readonly ILogger<ImportPdfFileCommandHandler> logger = logger;

        public async Task Handle(ImportPdfFileCommand command)
        {
            var _ = this.logger.BeginScope("ImportPdfFileCommandHandler");
            this.logger.LogInformation("Starting ImportPdfFileCommand handling for account: {AccountName}", command.AccountName);

            this.logger.LogInformation("Retrieving account: {AccountName}", command.AccountName);
            var account = await this.accountService.GetByNameAsync(command.AccountName);
            this.logger.LogInformation("Account retrieval completed for: {AccountName}", command.AccountName);

            if (account is null)
            {
                this.logger.LogInformation("Account not found, creating new account: {AccountName}", command.AccountName);
                account = await this.accountService.CreateAsync(command.AccountName);
                this.logger.LogInformation("Account created: {AccountName}", account.Name);
            }

            this.logger.LogInformation("Analyzing PDF file for account: {AccountName}", command.AccountName);
            var importData = await this.importService.ImportAsync(command.PdfStream);

            this.logger.LogInformation("Inserting categories");
            await this.categoryService.InsertIfNotExistsAsync(importData.Categories);

            this.logger.LogInformation("Inserting balances for account: {AccountName}", command.AccountName);
            var balances = await this.balanceService.InsertAsync(importData.Balances, command.AccountName);
            this.logger.LogInformation("Inserting transactions for account: {AccountName}", command.AccountName);
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
            this.logger.LogInformation("All transactions inserted successfully for account: {AccountName}", command.AccountName);
        }
    }
}
