using MyFinancialHub.Import.Domain.Entities.Accounts;
using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Domain.Entities
{
    public record class ImportData(
        List<Category> Categories,
        List<Balance> Balances,
        List<Transaction> Transactions
    );
}
