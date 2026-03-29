using TelemetryRecords.Models;
using TelemetryRecords.Repositories.Base.Interfaces;

namespace TelemetryRecords.Repositories.TelemetryDataRepository.Interfaces
{
    public interface ITelemetryDataRepository : IRepository<TelemetryDataPoint>
    {
        Task<long> CountByMissionAsync(
            double missionIdHash,
            int tailId,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken cancellationToken = default);

        Task<List<TelemetryDataPoint>> GetByMissionAsync(
            double missionIdHash,
            int tailId,
            List<string>? fields = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int skip = 0,
            int take = 0,
            CancellationToken cancellationToken = default);
    }
}
