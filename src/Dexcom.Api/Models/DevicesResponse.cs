using Newtonsoft.Json;
using System;

namespace Dexcom.Api.Models
{
    public sealed class DevicesResponse
    {
        [JsonProperty("devices")]
        public Device[] Devices { get; set; }
    }

    public sealed class Device
    {
        [JsonProperty("transmitterGeneration")]
        public string TransmitterGeneration { get; set; }

        [JsonProperty("displayDevice")]
        public string DisplayDevice { get; set; }

        [JsonProperty("lastUploadDate")]
        public DateTime LastUploadDate { get; set; }

        [JsonProperty("alertScheduleList")]
        public Alertschedulelist[] AlertScheduleList { get; set; } = new Alertschedulelist[] { };
    }

    public sealed class Alertschedulelist
    {
        [JsonProperty("alertScheduleSettings")]
        public Alertschedulesettings AlertScheduleSettings { get; set; }

        [JsonProperty("alertSettings")]
        public Alertsetting[] AlertSettings { get; set; } = new Alertsetting[] { };
    }

    public sealed class Alertschedulesettings
    {
        [JsonProperty("alertScheduleName")]
        public string AlertScheduleName { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("isDefaultSchedule")]
        public bool IsDefaultSchedule { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("daysOfWeek")]
        public string[] DaysOfWeek { get; set; }
    }

    public sealed class Alertsetting
    {
        [JsonProperty("systemTime")]
        public DateTime SystemTime { get; set; }

        [JsonProperty("displayTime")]
        public DateTime DisplayTime { get; set; }

        [JsonProperty("alertName")]
        public string AlertName { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("snooze")]
        public int? Snooze { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}