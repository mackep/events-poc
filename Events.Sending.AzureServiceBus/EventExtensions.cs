using System;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Events.Base;
using Events.Infrastructure.AzureServiceBus;

namespace Events.Sending.AzureServiceBus
{
    public static class EventExtensions
    {
        public static ServiceBusMessage AsServiceBusMessage(this Event evnt, string systemId)
        {
            var content = JsonSerializer.Serialize(evnt, evnt.GetType());
            var message = new ServiceBusMessage(content);

            message.ApplicationProperties.Add(evnt.EventId.AsServiceBusMessageProperty());
            message.ApplicationProperties.Add(evnt.GetType().AsServiceBusMessageProperty());
            message.ApplicationProperties.Add(systemId.AsServiceBusMessageProperty());
            
            //foreach(var (key, value) in evnt.Metadata)
            //    message.ApplicationProperties.Add(new KeyValuePair<string, object>(key, value));

            return message;
        }

        private static KeyValuePair<string, object> AsServiceBusMessageProperty(this Type type)
        {
            return new(EventHeaderConstants.EventNameHeaderKey, type.Name);
        }

        private static KeyValuePair<string, object> AsServiceBusMessageProperty(this string systemIdentifier)
        {
            return new(EventHeaderConstants.OriginHeaderKey, systemIdentifier);
        }

        private static KeyValuePair<string, object> AsServiceBusMessageProperty(this int eventId)
        {
            return new(EventHeaderConstants.EventIdHeaderKey, eventId);
        }
    }
}