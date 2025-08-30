using MyFinancialHub.Application.CQRS.Handlers.Commands;
using MyFinancialHub.Infra.Events.Producers;
using MyFinancialHub.Upload.Application.Extensions;
using MyFinancialHub.Upload.Domain;
using MyFinancialHub.Upload.Domain.Interfaces.Services;

namespace MyFinancialHub.Upload.Application.Handlers.UploadPdfFile
{
    internal class UploadPdfFileCommandHandler(
       IUploadDataService uploadDataService,
       IProducer<UploadData> producer,
       ILogger<UploadPdfFileCommandHandler> logger
    ) : ICommandHandler<UploadPdfFileCommand>
    {
        private readonly IUploadDataService uploadDataService = uploadDataService;
        private readonly IProducer<UploadData> producer = producer;
        private readonly ILogger<UploadPdfFileCommandHandler> logger = logger;

        private static string GenerateFileName(string accountName)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return $"{accountName}_{timestamp}.pdf";
        }

        public async Task Handle(UploadPdfFileCommand command)
        {
            var _ = logger.BeginScope("UploadPdfFileCommandHandler");
            this.logger.LogInformation("Starting PDF upload process for account: {AccountName}", command.AccountName);

            this.logger.LogInformation("Validating if the uploaded file is a PDF.");
            if (!command.PdfStream.IsPdf())
            {
                this.logger.LogError("The uploaded file is not a valid PDF.");
                throw new InvalidDataException("The uploaded file is not a valid PDF.");
            }
            this.logger.LogInformation("The uploaded file is a valid PDF.");

            this.logger.LogDebug("Generating file name for the uploaded PDF.");
            var fileName = GenerateFileName(command.AccountName);
            this.logger.LogDebug("Generated file name: {FileName}", fileName);

            this.logger.LogInformation("Uploading the PDF file.");
            var uploadResult = await this.uploadDataService.ProcessUploadAsync(fileName, command.PdfStream);
            this.logger.LogInformation("PDF file uploaded successfully. File URL: {FileUrl}", uploadResult.FileUrl);

            this.logger.LogInformation("Producing upload event to the message broker.");
            await this.producer.ProduceAsync(uploadResult);
            this.logger.LogInformation("Upload event produced successfully.");
        }
    }
}
