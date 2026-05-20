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
                    TotalCount = (long)allRos.Count,
                };
            }

            int skip = page * pageSize;

            Task<long> countTask = _telemetryDataRepository.CountByMissionAsync(
                missionIdHash,
                tailId,
                startTime,
                endTime,
                cancellationToken);

            Task<List<TelemetryDataPoint>> pageTask = _telemetryDataRepository.GetByMissionAsync(
                missionIdHash,
                tailId,
                fields,
                startTime,
                endTime,
                skip,
                pageSize,
                cancellationToken);

            await Task.WhenAll(countTask, pageTask);

            long totalCount = await countTask;
            List<TelemetryDataPoint> pagePoints = await pageTask;

            return new MissionTelemetryPageRo
            {
                Items = pagePoints.ToRo(),
                TotalCount = totalCount,
            };
        }

        public async Task<MissionTelemetryBoundsRo> GetMissionTelemetryBoundsAsync(
            string missionId,
            int tailId,
            CancellationToken cancellationToken = default)
        {
            double missionIdHash = MissionIdHashUtility.ToHash(missionId);
            MissionTelemetryTimeBounds bounds = await _telemetryDataRepository.GetMissionTelemetryTimeBoundsAsync(
                missionIdHash,
                tailId,
                cancellationToken);
            return MapToBoundsResponse(bounds);
        }

        private static MissionTelemetryBoundsRo MapToBoundsResponse(MissionTelemetryTimeBounds bounds)
        {
            if (bounds.TotalCount == 0)
            {
                return new MissionTelemetryBoundsRo { TotalCount = 0 };
            }

            return new MissionTelemetryBoundsRo
            {
                FirstTimestamp = bounds.FirstTimestamp,
                LastTimestamp = bounds.LastTimestamp,
                TotalCount = bounds.TotalCount,
            };
        }
    }
}
