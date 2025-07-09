using MyFinancialHub.Import.Infra.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Import.Application;
using MyFinancialHub.Import.Application.Handlers.ImportPdfFile;
using MyFinancialHub.Import.Infra.AI.DocumentIntelligence;

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(configure => configure.AddConsole());
serviceCollection
    .AddApplication()
    .AddSqlRepositories()
    .AddDocumentInteligence();
var serviceProvider = serviceCollection.BuildServiceProvider();

var service = serviceProvider.GetRequiredService<IDispatcher>();

var Main = async () =>
{
    try
    {
        var path = "C:\\Users\\Frank\\Desktop\\2017_lancamentos.pdf";
        var file = File.OpenRead(path);
        var accountName = "TestAccount";
        await service.Dispatch(new ImportPdfFileCommand(file, accountName));

        Console.WriteLine("Document analysis completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
    Console.ReadKey();
};

Main().GetAwaiter().GetResult();