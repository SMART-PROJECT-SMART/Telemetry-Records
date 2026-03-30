namespace TelemetryRecords.Models.Ro
{
    public class MissionTelemetryBoundsRo
    {
        public DateTime? FirstTimestamp { get; set; }
        public DateTime? LastTimestamp { get; set; }
        public long TotalCount { get; set; }
    }
}
