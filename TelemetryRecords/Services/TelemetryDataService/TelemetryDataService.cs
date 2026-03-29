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

        public async Task<MissionTelemetryPageRo> GetMissionTelemetryAsync(
            string missionId,
            int tailId,
            List<string>? fields = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int page = 0,
            int pageSize = 0,
            CancellationToken cancellationToken = default)
        {
            double missionIdHash = MissionIdHashUtility.ToHash(missionId);

            if (pageSize <= 0)
            {
                List<TelemetryDataPoint> allPoints = await _telemetryDataRepository.GetByMissionAsync(
                    missionIdHash,
                    tailId,
                    fields,
                    startTime,
                    endTime,
                    skip: 0,
                    take: 0,
                    cancellationToken);

                List<MissionTelemetryRo> allRos = allPoints.ToRo();
                return new MissionTelemetryPageRo
                {
                    Items = allRos,
                    TotalCount = allRos.Count,
                };
            }

            long totalCount = await _telemetryDataRepository.CountByMissionAsync(
                missionIdHash,
                tailId,
                startTime,
                endTime,
                cancellationToken);

            int skip = page * pageSize;
            List<TelemetryDataPoint> pagePoints = await _telemetryDataRepository.GetByMissionAsync(
                missionIdHash,
                tailId,
                fields,
                startTime,
                endTime,
                skip,
                pageSize,
                cancellationToken);

            return new MissionTelemetryPageRo
            {
                Items = pagePoints.ToRo(),
                TotalCount = totalCount,
            };
        }
    }
}
