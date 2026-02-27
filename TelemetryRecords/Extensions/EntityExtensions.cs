using TelemetryRecords.Models;
using TelemetryRecords.Models.Ro;

namespace TelemetryRecords.Extensions
{
    public static class EntityExtensions
    {
        public static MissionRo ToRo(this Mission mission)
        {
            return new MissionRo
            {
                Title = mission.Title,
                RequiredUAVType = mission.RequiredUAVType,
                Priority = mission.Priority,
                TimeWindow = mission.TimeWindow,
                Location = mission.Location
            };
        }

        public static MissionToUavAssignmentRo ToRo(this MissionToUavAssignment missionToUavAssignment)
        {
            return new MissionToUavAssignmentRo
            {
                Mission = missionToUavAssignment.Mission.ToRo(),
                UavTailId = missionToUavAssignment.UavTailId,
                StartTime = missionToUavAssignment.StartTime
            };
        }

        public static AssignmentRo ToRo(this Assignment assignment)
        {
            return new AssignmentRo
            {
                SuggestedAssignments = assignment.SuggestedAssignments.Select(x => x.ToRo()).ToList(),
                ActualAssignments = assignment.ActualAssignments.Select(x => x.ToRo()).ToList(),
                CreatedAt = assignment.CreatedAt
            };
        }

        public static IEnumerable<AssignmentRo> ToRo(this IEnumerable<Assignment> assignments)
        {
            return assignments.Select(x => x.ToRo());
        }
    }
}
