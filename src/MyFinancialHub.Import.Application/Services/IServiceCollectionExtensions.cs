namespace MyFinancialHub.Import.Application.Services
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<AccountService>()
                .AddScoped<BalanceService>()
                .AddScoped<CategoryService>()
                .AddScoped<TransactionService>(); 
        }
    }
}
