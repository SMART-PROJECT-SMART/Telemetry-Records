using Core.Common.Helpers;
using TelemetryRecords.Extensions;
using TelemetryRecords.Models;
using TelemetryRecords.Models.Ro;
using TelemetryRecords.Repositories.TelemetryDataRepository.Interfaces;
using TelemetryRecords.Services.TelemetryDataService.Interfaces;

namespace TelemetryRecords.Services.TelemetryDataService
{
    public class TelemetryDataService : ITelemetryDataService
    {
        private readonly ITelemetryDataRepository _telemetryDataRepository;

        public TelemetryDataService(ITelemetryDataRepository telemetryDataRepository)
        {
            _telemetryDataRepository = telemetryDataRepository;
        }

        public async Task<List<MissionTelemetryRo>> GetMissionTelemetryAsync(
            string missionId,
            int tailId,
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default)
        {
            double missionIdHash = MissionIdHashUtility.ToHash(missionId);

            List<TelemetryDataPoint> dataPoints = await _telemetryDataRepository.GetByMissionAsync(
                missionIdHash, tailId, startTime, endTime, cancellationToken);

            return dataPoints.ToRo();
        }
    }
}
