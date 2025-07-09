using MyFinancialHub.Import.Domain.Entities.Accounts;
using MyFinancialHub.Import.Infra.Data.Mappers;

namespace MyFinancialHub.Import.Infra.Data.Repositories
{
    internal class BalancesRepository(FinancialHubContext context, BalanceMapper mapper) : IBalanceRepository
    {
        private readonly FinancialHubContext context = context;
        private readonly BalanceMapper mapper = mapper;

        public async Task AddAsync(Balance account)
        {
            throw new NotImplementedException();
        }

        public async Task<Balance> GetByNameAsync(string name)
        {
            return await this.context.Balances
                .FirstOrDefaultAsync(balance => balance.Name == name)
                .ContinueWith(task => task.Result is null ? null : this.mapper.Map(task.Result));
        }

        public async Task UpdateAsync(Balance account)
        {
            throw new NotImplementedException();
        }
    }
}
