using System;
using Events.Base;

namespace Events.Contracts
{
    public record XCreated(Guid Id) : Event(101);
}
