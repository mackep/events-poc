using System;
using Events.Base;

namespace Events.Handling
{
    public class EventHandlingException : Exception
    {
        public EventHandlingException(Event evnt, Exception ex) : base($"Failed to handle {evnt.GetType().Name} event", ex)
        {
        }
    }
}
