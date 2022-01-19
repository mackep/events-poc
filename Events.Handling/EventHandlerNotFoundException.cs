using System;

namespace Events.Handling
{
    public class EventHandlerNotFoundException : Exception
    {
        public EventHandlerNotFoundException(Type eventType) : base($"No event handler for {eventType.Name} could be found")
        {
        }
    }
}