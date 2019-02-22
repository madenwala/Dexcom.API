using Newtonsoft.Json;

namespace Dexcom.Api.Models
{
    public class StatisticsRequest
    {
        [JsonProperty("targetRanges")]
        public TargetRange[] TargetRanges { get; set; } = new TargetRange[] { };
    }

    public class TargetRange
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("egvRanges")]
        public EgvRange[] EgvRanges { get; set; }
    }

    public class EgvRange
    {
        [JsonProperty("name")]
        public EgvRangeNames Name { get; set; }

        [JsonProperty("bound")]
        public float Bound { get; set; }
    }

    public enum EgvRangeNames
    {
        [JsonProperty("urgentLow")]
        UrgentLow,

        [JsonProperty("low")]
        Low,

        [JsonProperty("high")]
        High
    }
}