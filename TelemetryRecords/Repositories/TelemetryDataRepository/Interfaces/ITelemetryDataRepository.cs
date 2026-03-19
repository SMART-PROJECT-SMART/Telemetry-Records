using TelemetryRecords.Models;
using TelemetryRecords.Repositories.Base.Interfaces;

namespace TelemetryRecords.Repositories.TelemetryDataRepository.Interfaces
{
    public interface ITelemetryDataRepository : IRepository<TelemetryDataPoint>
    {
        Task<List<TelemetryDataPoint>> GetByMissionAsync(
            double missionIdHash,
            int tailId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default);
    }
}
