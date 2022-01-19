using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Handling.AzureServiceBus
{
    public static class SetupExtensions
    {
        public static IServiceCollection AddAzureServiceBusEventListening(this IServiceCollection services, IEnumerable<Type> eventTypes)
        {
            services.AddSingleton(_ => new ServiceBusEventDeserializer(eventTypes));
            services.AddSingleton<IEventMediator, EventMediator>();

            return services;
        }
    }
}
