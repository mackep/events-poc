using Microsoft.Extensions.DependencyInjection;

namespace Events.Sending.AzureServiceBus
{
    public static class SetupExtensions
    {
        public static IServiceCollection AddAzureServiceBusEventSending(this IServiceCollection services)
        {
            services.AddSingleton<IEventSender, ServiceBusEventSender>();

            return services;
        }
    }
}