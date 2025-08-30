using MyFinancialHub.Application.CQRS.Handlers.Queries;

namespace MyFinancialHub.Import.Application.Handlers.DownloadPdfFile
{
    internal class DownloadPdfFileQueryHandler(
        ILogger<DownloadPdfFileQueryHandler> logger
    ) : IQueryHandler<DownloadPdfFileQuery, Stream>
    {
        private readonly ILogger<DownloadPdfFileQueryHandler> logger = logger;

        public async Task<Stream> Handle(DownloadPdfFileQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
