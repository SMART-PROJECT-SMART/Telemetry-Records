using TelemetryRecords.Models.Ro;

namespace TelemetryRecords.Services.TelemetryDataService.Interfaces
{
    public interface ITelemetryDataService
    {
        Task<List<MissionTelemetryRo>> GetMissionTelemetryAsync(
            string missionId,
            int tailId,
            List<string>? fields = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken cancellationToken = default);
    }
}
