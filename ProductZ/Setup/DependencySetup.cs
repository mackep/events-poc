using Client;
using Events.Contracts;
using Events.Handling;
using Events.Handling.AzureServiceBus;
using Events.Infrastructure.AzureServiceBus;
using Events.Sending.AzureServiceBus;
using Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace ProductZ.Setup
{
    public static class DependencySetup
    {
        public static void AddEventHandling(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AzureServiceBusConfiguration>(config.GetSection("EventBus"));
            services.AddAzureServiceBusEventListening(EventTypes.AllInContractsLibrary());

            services.AddHttpClient();
            services.AddSingleton<XApiClient>();
            services.AddSingleton<YApiClient>();

            services.AddEventHandler<XCreatedEventHandler>();
            services.AddEventHandler<XUpdatedEventHandler>();
            services.AddEventHandler<YCreatedEventHandler>();
            services.AddEventHandler<YUpdatedEventHandler>();
        }

        public static void AddEventSending(this IServiceCollection services)
        {
            services.AddAzureServiceBusEventSending();
        }

        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryCharacterRepository>();
            services.AddSingleton<ICharacterRepository>(provider =>
                new StateLoggingDecorator(
                    provider.GetRequiredService<InMemoryCharacterRepository>()));
        }
    }
}
