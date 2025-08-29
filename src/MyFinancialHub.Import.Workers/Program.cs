using MyFinancialHub.Import.Workers.BackgroundServices;
using MyFinancialHub.Import.Infra.Events.Consumers;

namespace MyFinancialHub.Import.Workers
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddLogging();
            builder.Services.AddImportBalanceDataConsumer();
            builder.Services.AddHostedService<ImportBalanceDataWorker>();

            var host = builder.Build();
            host.Run();
        }
    }
}