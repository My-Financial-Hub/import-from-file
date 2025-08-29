namespace MyFinancialHub.Infra.Events.Producers
{
    public interface IProducer<in T>
    {
        Task ProduceAsync(T message);
    }
}
