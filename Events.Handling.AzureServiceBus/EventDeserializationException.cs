using System;

namespace Events.Handling.AzureServiceBus
{
    public class EventDeserializationException : Exception
    {
        public EventDeserializationException(string msg) : base($"Event deserialization failed. {msg}")
        {
        }
    }
}