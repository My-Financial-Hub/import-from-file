namespace MyFinancialHub.Application.CQRS.Handlers.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class
    {
        Task Handle(TCommand command);
    }
}
