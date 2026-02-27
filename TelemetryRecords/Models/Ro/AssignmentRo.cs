namespace TelemetryRecords.Models.Ro
{
    public class AssignmentRo
    {
        public List<MissionToUavAssignmentRo> SuggestedAssignments { get; set; }
        public List<MissionToUavAssignmentRo> ActualAssignments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
