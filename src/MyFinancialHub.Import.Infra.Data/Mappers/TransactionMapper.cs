using MyFinancialHub.Import.Domain.Entities.Transactions;

namespace MyFinancialHub.Import.Infra.Data.Mappers
{
    internal class TransactionMapper
    {
        public TransactionEntity Map(Transaction transaction)
        {
            if (transaction == null)
                return null;

            var entity = new TransactionEntity
            {
                TargetDate = transaction.Date,
                FinishDate = transaction.Date,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Status = transaction.Status,
                Type = transaction.Type,
                IsActive = true 
            };
            return entity;
        }
    }
}
