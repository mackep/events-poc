using System;
using Events.Base;

namespace Events.Contracts
{
    public record XUpdated(Guid Id) : Event(102);
}