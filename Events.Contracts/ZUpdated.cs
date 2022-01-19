using System;
using Events.Base;

namespace Events.Contracts
{
    public record ZUpdated(Guid Id) : Event(106);
}