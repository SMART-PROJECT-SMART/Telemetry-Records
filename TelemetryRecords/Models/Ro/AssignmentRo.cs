namespace TelemetryRecords.Models.Ro
{
    public class AssignmentRo
    {
        public required List<MissionToUavAssignmentRo> SuggestedAssignments { get; set; }
        public required List<MissionToUavAssignmentRo> ActualAssignments { get; set; }
        public Dictionary<string, Dictionary<string, double>> AllUavTelemetryData { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
