using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Events.Base;
using Events.Infrastructure.AzureServiceBus;

namespace Events.Handling.AzureServiceBus
{
    public class ServiceBusEventDeserializer
    {
        private readonly Dictionary<string, Type> _eventTypes;

        public ServiceBusEventDeserializer(IEnumerable<Type> eventTypes)
        {
            _eventTypes = eventTypes.ToDictionary(t => t.Name, t => t);

            if (_eventTypes == null || !_eventTypes.Any())
                throw new ApplicationException("Cannot initialize event deserializer without any event types");
        }

        public Event Deserialize(ServiceBusReceivedMessage message)
        {
            var eventName = (string) message.ApplicationProperties[EventHeaderConstants.EventNameHeaderKey];

            if (!_eventTypes.ContainsKey(eventName))
                throw new EventDeserializationException($"Event of type {eventName} is not listed among known event types");

            var eventType = _eventTypes[eventName];

            return (Event) JsonSerializer.Deserialize(message.Body.ToString(), eventType);
        }
    }
}