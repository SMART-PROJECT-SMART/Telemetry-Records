namespace TelemetryRecords.Models.Ro
{
    public class AssignmentRo
    {
        public required List<MissionToUavAssignmentRo> SuggestedAssignments { get; set; }
        public required List<MissionToUavAssignmentRo> ActualAssignments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
