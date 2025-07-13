namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal abstract class BaseRepository(FinancialHubContext context)
    {
        protected readonly FinancialHubContext context = context;
        public async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
