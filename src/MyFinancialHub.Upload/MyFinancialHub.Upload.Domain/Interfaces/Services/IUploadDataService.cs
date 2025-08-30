namespace MyFinancialHub.Upload.Domain.Interfaces.Services
{
    public interface IUploadDataService
    {
        Task<UploadData> ProcessUploadAsync(string dataName, Stream data);
    }
}
