using Newtonsoft.Json;
using System;

namespace Dexcom.Api.Models
{
    public sealed class DataRangeResponse
    {
        [JsonProperty("calibrations")]
        public DataRange Calibrations { get; set; }

        [JsonProperty("egvs")]
        public DataRange EGVs { get; set; }

        [JsonProperty("events")]
        public DataRange Events { get; set; }
    }

    public sealed class DataRange
    {
        [JsonProperty("start")]
        public DataRangeDates Start { get; set; }

        [JsonProperty("end")]
        public DataRangeDates End { get; set; }
    }

    public sealed class DataRangeDates
    {
        [JsonProperty("systemTime")]
        public DateTime SystemTime { get; set; }

        [JsonProperty("displayTime")]
        public DateTime DisplayTime { get; set; }
    }
}