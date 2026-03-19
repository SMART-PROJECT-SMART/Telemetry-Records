using Core.Common.Enums;
using Core.Models;
using TelemetryRecords.Common.Enums;

namespace TelemetryRecords.Models.Ro
{
    public class MissionRo
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public UAVType RequiredUAVType { get; set; }
        public MissionPriority Priority { get; set; }
        public TimeWindow TimeWindow { get; set; }
        public Location Location { get; set; }
    }
}
