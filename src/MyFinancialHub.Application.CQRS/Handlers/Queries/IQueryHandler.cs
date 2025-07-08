namespace MyFinancialHub.Application.CQRS.Handlers.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : class
    {
        Task<TResult> Handle(TQuery query);
    }
}
