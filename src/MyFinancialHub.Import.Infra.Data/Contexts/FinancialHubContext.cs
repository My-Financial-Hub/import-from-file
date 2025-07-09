using System.Diagnostics.CodeAnalysis;

namespace MyFinancialHub.Import.Infra.Data.Contexts
{
    internal class FinancialHubContext([NotNull] DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialHubContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<BalanceEntity> Balances { get; set; }
    }
}
