using Newtonsoft.Json;
using System;

namespace Dexcom.Api.Models
{
    public class EventsResponse
    {
        [JsonProperty("events")]
        public Event[] Events { get; set; } = new Event[] { };
    }

    public class Event
    {
        [JsonProperty("systemTime")]
        public string SystemTime { get; set; }

        [JsonProperty(PropertyName = "displayTime", ItemConverterType = typeof(DateTimeConverter))]
        public DateTime DisplayTime { get; set; }

        [JsonProperty("eventType")]
        public EventTypes EventType { get; set; }

        [JsonProperty("eventSubType")]
        public string EventSubType { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("eventId")]
        public string EventID { get; set; }

        [JsonProperty("eventStatus")]
        public EventStatuses EventStatus { get; set; }
    }

    public enum EventTypes
    {
        [JsonProperty("carbs")]
        Carbs,

        [JsonProperty("insulin")]
        Insulin,

        [JsonProperty("exercise")]
        Exercise,

        [JsonProperty("health")]
        Health
    }

    public enum EventStatuses
    {
        [JsonProperty("created")]
        Created,

        [JsonProperty("deleted")]
        Deleted
    }
}