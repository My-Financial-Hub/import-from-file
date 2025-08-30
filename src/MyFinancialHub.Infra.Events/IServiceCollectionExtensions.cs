namespace MyFinancialHub.Infra.Events
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventConsumers(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddEventProducers(this IServiceCollection services)
        {
            return services;
        }
    }
}
