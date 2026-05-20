namespace TelemetryRecords.Models.Ro
{
    public class MissionTelemetryRo
    {
        public DateTime Timestamp { get; set; }
        public int TailId { get; set; }
        public Dictionary<string, double> Fields { get; set; } = new();
    }
}
