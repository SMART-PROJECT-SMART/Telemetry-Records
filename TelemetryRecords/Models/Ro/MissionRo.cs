using Core.Common.Enums;
using Core.Models;
using TelemetryRecords.Common.Enums;

namespace TelemetryRecords.Models.Ro
{
    public class MissionRo
    {
        public UAVType RequiredUAVType { get; set; }
        public MissionPriority Priority { get; set; }
        public TimeWindow TimeWindow { get; set; }
        public Location Location { get; set; }
    }
}
