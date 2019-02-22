using Newtonsoft.Json;

namespace Dexcom.Api.Models
{
    public sealed class StatisticsResponse
    {
        [JsonProperty("hypoglycemiaRisk")]
        public string HypoglycemiaRisk { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("mean")]
        public float Mean { get; set; }

        [JsonProperty("median")]
        public int Median { get; set; }

        [JsonProperty("variance")]
        public float Variance { get; set; }

        [JsonProperty("stdDev")]
        public float StdDev { get; set; }

        [JsonProperty("sum")]
        public int Sum { get; set; }

        [JsonProperty("q1")]
        public int Q1 { get; set; }

        [JsonProperty("q2")]
        public int Q2 { get; set; }

        [JsonProperty("q3")]
        public int Q3 { get; set; }

        [JsonProperty("utilizationPercent")]
        public float UtilizationPercent { get; set; }

        [JsonProperty("meanDailyCalibrations")]
        public float MeanDailyCalibrations { get; set; }

        [JsonProperty("nDays")]
        public int NDays { get; set; }

        [JsonProperty("nValues")]
        public int NValues { get; set; }

        [JsonProperty("nUrgentLow")]
        public int NUrgentLow { get; set; }

        [JsonProperty("nBelowRange")]
        public int NBelowRange { get; set; }

        [JsonProperty("nWithinRange")]
        public int NWithinRange { get; set; }

        [JsonProperty("nAboveRange")]
        public int NAboveRange { get; set; }

        [JsonProperty("percentUrgentLow")]
        public float PercentUrgentLow { get; set; }

        [JsonProperty("percentBelowRange")]
        public float PercentBelowRange { get; set; }

        [JsonProperty("percentWithinRange")]
        public float PercentWithinRange { get; set; }

        [JsonProperty("percentAboveRange")]
        public float PercentAboveRange { get; set; }
    }
}