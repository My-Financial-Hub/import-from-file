using MyFinancialHub.Import.Application.Services;

namespace MyFinancialHub.Import.Application.Handlers.ImportPdfFile
{
    internal class ImportPdfFileCommandHandler(
        AccountService accountService,
        IImportDataService importService,
        ImportDataService importDataService,
        ILogger<ImportPdfFileCommandHandler> logger
    ) : ICommandHandler<ImportPdfFileCommand>
    {
        private readonly AccountService accountService = accountService;
        private readonly IImportDataService importService = importService;
        private readonly ImportDataService importDataService = importDataService;
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
                this.logger.LogInformation("Account created: {AccountName}", command.AccountName);
            }

            this.logger.LogInformation("Analyzing PDF file for account: {AccountName}", account.Name);
            var importData = await this.importService.ImportAsync(command.PdfStream);
            this.logger.LogInformation("PDF analysis completed for account: {AccountName}", account.Name);

            this.logger.LogInformation("Importing data for account: {AccountName}", account.Name);
            await this.importDataService.ImportAsync(importData, account);
            this.logger.LogInformation("Import completed for account: {AccountName}", account.Name);

            this.logger.LogInformation("ImportPdfFileCommand handling completed for account: {AccountName}", account.Name);
        }
    }
}
