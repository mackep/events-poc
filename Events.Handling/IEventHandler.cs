using System;
using System.Threading.Tasks;
using Events.Base;

namespace Events.Handling
{
    public interface IEventHandler
    {
        Type EventType { get; }

        Task Handle(Event evnt);
    }

    public interface IEventHandler<in T> : IEventHandler
        where T : Event
    {
        Task Handle(T evnt);
    }
}