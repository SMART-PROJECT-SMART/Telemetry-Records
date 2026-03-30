using TelemetryRecords.Models.Ro;

namespace TelemetryRecords.Services.TelemetryDataService.Interfaces
{
    public interface ITelemetryDataService
    {
        Task<MissionTelemetryPageRo> GetMissionTelemetryAsync(
            string missionId,
            int tailId,
            List<string>? fields = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int page = 0,
            int pageSize = 0,
            CancellationToken cancellationToken = default);

        Task<MissionTelemetryBoundsRo> GetMissionTelemetryBoundsAsync(
            string missionId,
            int tailId,
            CancellationToken cancellationToken = default);
    }
}
