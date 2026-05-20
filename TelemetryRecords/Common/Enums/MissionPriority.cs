using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TelemetryRecords.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MissionPriority
    {
        Low,
        Medium,
        High,
    }
}
