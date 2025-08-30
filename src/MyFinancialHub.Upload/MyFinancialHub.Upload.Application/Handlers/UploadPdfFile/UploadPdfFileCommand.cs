namespace MyFinancialHub.Upload.Application.Handlers.UploadPdfFile
{
    public record class UploadPdfFileCommand(Stream PdfStream, string AccountName);
}
