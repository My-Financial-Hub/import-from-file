namespace MyFinancialHub.Infra.Events.Handlers
{
    public interface IEventHandler<in T>
    {
        Task HandleAsync(T @event);
    }
}
