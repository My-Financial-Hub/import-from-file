namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal abstract class BaseRepository(
        FinancialHubContext context, 
        ILogger<BaseRepository> logger
    )
    {
        protected readonly FinancialHubContext context = context;
        protected readonly ILogger<BaseRepository> logger = logger;

        public async Task CommitAsync()
        {
            this.logger.LogDebug("Committing changes to the repository");

            this.logger.LogTrace("Saving changes to the context");
            var affectedRows = await this.context.SaveChangesAsync();
            this.logger.LogTrace("Changes saved to the context");

            this.logger.LogDebug("Committed {AffectedRows} changes to the repository", affectedRows);
        }
    }
}
