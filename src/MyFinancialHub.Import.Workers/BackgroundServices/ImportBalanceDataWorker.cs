using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Import.Application.Handlers.DownloadPdfFile;
using MyFinancialHub.Import.Application.Handlers.ImportPdfFile;
using MyFinancialHub.Import.Domain.Entities;
using MyFinancialHub.Infra.Events.Consumers;

namespace MyFinancialHub.Import.Workers.BackgroundServices
{
    public class ImportBalanceDataWorker(
        IConsumer<ImportDataEvent> consumer,
        IDispatcher dispatcher,
        ILogger<ImportBalanceDataWorker> logger
    ) : BackgroundService
    {
        private readonly IConsumer<ImportDataEvent> consumer = consumer;
        private readonly IDispatcher dispatcher = dispatcher;
        private readonly ILogger<ImportBalanceDataWorker> logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);

                var message = await this.consumer.ConsumeAsync();
                if (message is not null)
                {
                    var file = await this.dispatcher.Dispatch<DownloadPdfFileQuery, Stream>(
                        new DownloadPdfFileQuery(message.FilePath)
                    );

                    await this.dispatcher.Dispatch(
                        new ImportPdfFileCommand(file, message.AccountName)
                    );
                }

                await Task.Delay(1000, stoppingToken);
            } 
        }
    }
}