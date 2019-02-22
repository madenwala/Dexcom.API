using Newtonsoft.Json;
using System;

namespace Dexcom.Api.Models
{
    public sealed class CalibrationsResponse
    {
        [JsonProperty("calibrations")]
        public Calibration[] Calibrations { get; set; } = new Calibration[] { };
    }

    public sealed class Calibration
    {
        [JsonProperty("systemTime")]
        public DateTime SystemTime { get; set; }

        [JsonProperty("displayTime")]
        public DateTime DisplayTime { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }
    }
}