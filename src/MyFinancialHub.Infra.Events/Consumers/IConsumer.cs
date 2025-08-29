namespace MyFinancialHub.Infra.Events.Consumers
{
    public interface IConsumer<T>
    {
        Task<T> ConsumeAsync();
    }
}
