using System;
using Events.Base;

namespace Events.Contracts
{
    public record ZCreated(Guid Id) : Event(105);
}