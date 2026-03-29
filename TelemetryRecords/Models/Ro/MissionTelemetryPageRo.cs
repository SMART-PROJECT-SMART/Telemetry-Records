namespace TelemetryRecords.Models.Ro
{
    public class MissionTelemetryPageRo
    {
        public List<MissionTelemetryRo> Items { get; set; } = new();
        public long TotalCount { get; set; }
    }
}
