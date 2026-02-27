using MongoDB.Bson.Serialization.Attributes;

namespace TelemetryRecords.Models
{
    public class MissionToUavAssignment
    {
        [BsonRequired]
        public required Mission Mission { get; set; }

        [BsonRequired]
        public int UavTailId { get; set; }

        [BsonRequired]
        public DateTime StartTime { get; set; }
    }
}
