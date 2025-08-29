namespace MyFinancialHub.Import.Infra.Events.Consumers
{
    internal class ImportBalanceDataConsumerConfigurations
    {
        public string ServiceBusConnectionString { get; init; } = string.Empty;//TODO: move outside I guess 
        public string TopicName { get; init; } = "topic"; 
        public string SubscriptionName { get; init; } = "subscription";
        public int MaxConcurrentCalls { get; init; } = 1; 
    }
}
