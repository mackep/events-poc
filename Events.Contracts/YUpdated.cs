using System;
using Events.Base;

namespace Events.Contracts
{
    public record YUpdated : VersionedEntityEvent
    {
        public Guid Id { get; }
        public char NewValue { get; }

        public YUpdated(Guid id, char newValue, long entityVersion) : base(104, entityVersion)
        {
            Id = id;
            NewValue = newValue;
        }
    }
}