using MongoDB.Bson;
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
                Id = mission.Id,
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
                StartTime = missionToUavAssignment.StartTime,
                UavTelemetrySnapshot = missionToUavAssignment.UavTelemetrySnapshot
            };
        }

        public static AssignmentRo ToRo(this Assignment assignment)
        {
            return new AssignmentRo
            {
                SuggestedAssignments = assignment.SuggestedAssignments.Select(x => x.ToRo()).ToList(),
                ActualAssignments = assignment.ActualAssignments.Select(x => x.ToRo()).ToList(),
                AllUavTelemetryData = assignment.AllUavTelemetryData,
                CreatedAt = assignment.CreatedAt
            };
        }

        public static IEnumerable<AssignmentRo> ToRo(this IEnumerable<Assignment> assignments)
        {
            return assignments.Select(x => x.ToRo());
        }

        public static MissionTelemetryRo ToRo(this TelemetryDataPoint dataPoint)
        {
            Dictionary<string, double> fields = new Dictionary<string, double>();

            foreach (BsonElement element in dataPoint.TelemetryData)
            {
                if (element.Value.IsDouble)
                    fields[element.Name] = element.Value.AsDouble;
                else if (element.Value.IsInt32)
                    fields[element.Name] = element.Value.AsInt32;
                else if (element.Value.IsInt64)
                    fields[element.Name] = element.Value.AsInt64;
            }

            return new MissionTelemetryRo
            {
                Timestamp = dataPoint.Timestamp,
                TailId = dataPoint.TailId,
                Fields = fields
            };
        }

        public static List<MissionTelemetryRo> ToRo(this List<TelemetryDataPoint> dataPoints)
        {
            return dataPoints.Select(x => x.ToRo()).ToList();
        }
    }
}
