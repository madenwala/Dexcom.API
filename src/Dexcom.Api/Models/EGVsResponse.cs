using Newtonsoft.Json;
using System;

namespace Dexcom.Api.Models
{
    public sealed class EGVsResponse
    {
        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("rateUnit")]
        public string RateUnit { get; set; }

        [JsonProperty("egvs")]
        public EGV[] EGVs { get; set; } = new EGV[] { };
    }

    public sealed class EGV
    {
        [JsonProperty("systemTime")]
        public DateTime SystemTime { get; set; }

        [JsonProperty("displayTime")]
        public DateTime DisplayTime { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("realtimeValue")]
        public float RealtimeValue { get; set; }

        [JsonProperty("smoothedValue")]
        public float? SmoothedValue { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("trend")]
        public string Trend { get; set; }

        [JsonProperty("trendRate")]
        public float? TrendRate { get; set; }
    }
}