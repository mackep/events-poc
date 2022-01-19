using System.Text.Json.Serialization;

namespace Events.Base
{
    public abstract record VersionedEntityEvent : Event
    {
        public long EntityVersion { get; }

        protected VersionedEntityEvent(int eventId, long entityVersion) : base(eventId)
        {
            EntityVersion = entityVersion;
        }
    }

    public abstract record Event
    {
        [JsonIgnore]
        public int EventId { get; }

        protected Event(int eventId)
        {
            EventId = eventId;
        }
    }
}
