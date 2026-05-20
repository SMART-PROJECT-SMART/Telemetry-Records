using Core.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

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

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<TelemetryFields, double> UavTelemetrySnapshot { get; set; } = new();
    }
}
