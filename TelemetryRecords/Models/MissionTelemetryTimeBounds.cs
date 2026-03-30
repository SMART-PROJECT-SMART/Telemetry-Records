namespace TelemetryRecords.Models
{
    public sealed class MissionTelemetryTimeBounds
    {
        public DateTime FirstTimestamp { get; init; }
        public DateTime LastTimestamp { get; init; }
        public long TotalCount { get; init; }

        public static MissionTelemetryTimeBounds Empty { get; } = new() { TotalCount = 0 };
    }
}
