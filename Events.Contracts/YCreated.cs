using System;
using Events.Base;

namespace Events.Contracts
{
    public record YCreated : VersionedEntityEvent
    {
        public Guid Id { get; }
        public char NewValue { get; }

        public YCreated(Guid id, char newValue, long entityVersion) : base(103, entityVersion)
        {
            Id = id;
            NewValue = newValue;
        }
    }
}