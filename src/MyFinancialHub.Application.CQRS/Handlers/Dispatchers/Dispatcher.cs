using MyFinancialHub.Application.CQRS.Handlers.Commands;
using MyFinancialHub.Application.CQRS.Handlers.Queries;

namespace MyFinancialHub.Application.CQRS.Handlers.Dispatchers
{
    public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public async Task Dispatch<TCommand>(TCommand command) where TCommand : class
        {
            var handler = this.serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.Handle(command);
        }

        public async Task<TResult> Dispatch<TQuery, TResult>(TQuery query) where TQuery : class
        {
            var handler = this.serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await handler.Handle(query);
        }
    }
}
