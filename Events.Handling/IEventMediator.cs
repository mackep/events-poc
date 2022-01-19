using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events.Base;

namespace Events.Handling
{
    public interface IEventMediator
    {
        IReadOnlyCollection<Type> SupportedEventTypes { get; }

        Task Handle(Event evnt);
    }
}