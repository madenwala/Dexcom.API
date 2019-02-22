using Newtonsoft.Json;

namespace Dexcom.Api.Models
{
    public class Statistics
    {
        [JsonProperty("targetRanges")]
        public TargetRange[] TargetRanges { get; set; }
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
        public string Name { get; set; }

        [JsonProperty("bound")]
        public int Bound { get; set; }
    }
}