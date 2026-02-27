namespace TelemetryRecords.Models.Ro
{
    public class MissionToUavAssignmentRo
    {
        public MissionRo Mission { get; set; }
        public int UavTailId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
