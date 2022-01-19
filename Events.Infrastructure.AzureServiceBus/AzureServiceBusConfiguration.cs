namespace Events.Infrastructure.AzureServiceBus
{
    public class AzureServiceBusConfiguration
    {
        public string SendConnectionString { get; set; }
        public string ReceiveConnectionString { get; set; }
        public string SystemId { get; set; }
        public string Topic { get; set; }
    }
}
