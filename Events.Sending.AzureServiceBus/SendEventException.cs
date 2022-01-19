using System;
using System.Collections.Generic;
using System.Linq;
using Events.Base;

namespace Events.Sending.AzureServiceBus
{
    public class SendEventException : Exception
    {
        public SendEventException(Event evnt, Exception inner) : base($"Failed to send {evnt.GetType().Name} - {inner.Message}", inner)
        {
        }

        public SendEventException(IEnumerable<Event> events, Exception inner) : base($"Failed to send batch of events - {inner.Message}. The following events were included in the batch: {string.Join(",", events.Select(e => e.GetType().Name))}", inner)
        {
        }
    }
}