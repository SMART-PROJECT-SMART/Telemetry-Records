using TelemetryRecords.Models.Ro;

namespace TelemetryRecords.Services.TelemetryDataService.Interfaces
{
    public interface ITelemetryDataService
    {
        Task<List<MissionTelemetryRo>> GetMissionTelemetryAsync(
            string missionId,
            int tailId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default);
    }
}
