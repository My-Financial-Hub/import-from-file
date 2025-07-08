using MyFinancialHub.Application.CQRS.Handlers.Commands;
using MyFinancialHub.Import.Application.Services;

namespace MyFinancialHub.Import.Application.Handlers.ImportPdfFile
{
    internal class ImportPdfFileCommandHandler(
        AccountService accountService,
        BalanceService balanceService,
        IBalanceImportService documentImport,
        ILogger<ImportPdfFileCommandHandler> logger
    ) : ICommandHandler<ImportPdfFileCommand>
    {
        private readonly AccountService accountService = accountService;
        private readonly BalanceService balanceService = balanceService;
        private readonly IBalanceImportService documentImport = documentImport;
        private readonly ILogger<ImportPdfFileCommandHandler> logger = logger;

        public async Task Handle(ImportPdfFileCommand command)
        {
            var _ = this.logger.BeginScope("ImportPdfFileCommandHandler");
            this.logger.LogInformation("Handling ImportPdfFileCommand for account: {AccountName}", command.AccountName);
            var account = await this.accountService.GetOrCreateAsync(command.AccountName);

            this.logger.LogInformation("Analyzing PDF file for account: {AccountName}", command.AccountName);
            var balances = await this.documentImport.ImportAsync(command.PdfStream);

            this.logger.LogInformation("Inserting balances for account: {AccountName}", command.AccountName);
            balances = await this.balanceService.InsertAsync(balances);

            this.logger.LogInformation("Updating account with new balances: {AccountName}", command.AccountName);
            account.AddBalances(balances);
            await this.accountService.UpdateAsync(account);
        }
    }
}
