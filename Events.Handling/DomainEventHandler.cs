using System;
using System.Threading.Tasks;
using Events.Base;

namespace Events.Handling
{
    public abstract class DomainEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : Event
    {
        public Type EventType => typeof(TEvent);

        public async Task Handle(Event evnt)
        {
            await Handle((TEvent) evnt);
        }

        public abstract Task Handle(TEvent evnt);
    }
}