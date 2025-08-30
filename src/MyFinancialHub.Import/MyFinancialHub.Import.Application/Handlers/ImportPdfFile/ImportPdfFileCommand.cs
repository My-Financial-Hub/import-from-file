namespace MyFinancialHub.Import.Application.Handlers.ImportPdfFile
{
    public record class ImportPdfFileCommand(Stream PdfStream ,string AccountName);
}
