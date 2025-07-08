namespace MyFinancialHub.Import.Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByNameAsync(string name);
        Task CreateAsync(Account account);
        Task UpdateAsync(Account account);
    }
}
