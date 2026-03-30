using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models;
using TelemetryRecords.Models.Config;
using TelemetryRecords.Repositories.Base;
using TelemetryRecords.Repositories.TelemetryDataRepository.Interfaces;

namespace TelemetryRecords.Repositories.TelemetryDataRepository
{
    public class TelemetryDataRepository : BaseRepository<TelemetryDataPoint>, ITelemetryDataRepository
    {
        protected override string CollectionName => TelemetryRecordsConstants.Collections.TELEMETRY_COLLECTION;
        protected override string TimestampFieldName => TelemetryRecordsConstants.Fields.TIMESTAMP;

        public TelemetryDataRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfig)
            : base(mongoClient, mongoDbConfig)
        {
        }

        public async Task<long> CountByMissionAsync(
            double missionIdHash,
            int tailId,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken cancellationToken = default)
        {
            FilterDefinition<TelemetryDataPoint> filter = BuildMissionFilter(missionIdHash, tailId, startTime, endTime);
            return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task<List<TelemetryDataPoint>> GetByMissionAsync(
            double missionIdHash,
            int tailId,
            List<string>? fields = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int skip = 0,
            int take = 0,
            CancellationToken cancellationToken = default)
        {
            FilterDefinition<TelemetryDataPoint> filter = BuildMissionFilter(missionIdHash, tailId, startTime, endTime);

            IFindFluent<TelemetryDataPoint, TelemetryDataPoint> query = _collection
                .Find(filter)
                .Sort(Builders<TelemetryDataPoint>.Sort.Ascending(x => x.Timestamp));

            if (fields is { Count: > 0 })
                query = query.Project<TelemetryDataPoint>(BuildFieldProjection(fields));

            if (skip > 0)
                query = query.Skip(skip);

            if (take > 0)
                query = query.Limit(take);

            return await query.ToListAsync(cancellationToken);
        }

        private static FilterDefinition<TelemetryDataPoint> BuildMissionFilter(
            double missionIdHash,
            int tailId,
            DateTime? startTime,
            DateTime? endTime)
        {
            FilterDefinitionBuilder<TelemetryDataPoint> fb = Builders<TelemetryDataPoint>.Filter;

            FilterDefinition<TelemetryDataPoint> filter = fb.And(
                fb.Eq(x => x.TailId, tailId),
                fb.Eq(TelemetryRecordsConstants.Fields.MISSION_ID_PATH, missionIdHash));

            if (startTime.HasValue)
                filter &= fb.Gte(x => x.Timestamp, startTime.Value);

            if (endTime.HasValue)
                filter &= fb.Lte(x => x.Timestamp, endTime.Value);

            return filter;
        }

        public async Task<MissionTelemetryTimeBounds> GetMissionTelemetryTimeBoundsAsync(
            double missionIdHash,
            int tailId,
            CancellationToken cancellationToken = default)
        {
            FilterDefinition<TelemetryDataPoint> filter = BuildMissionFilter(missionIdHash, tailId, null, null);

            MissionTelemetryTimeBounds? row = await _collection.Aggregate()
                .Match(filter)
                .Group(
                    _ => TelemetryRecordsConstants.TelemetryAggregation.GroupAllDocumentsKey,
                    g => new MissionTelemetryTimeBounds
                    {
                        FirstTimestamp = g.Min(x => x.Timestamp),
                        LastTimestamp = g.Max(x => x.Timestamp),
                        TotalCount = g.Count(),
                    })
                .FirstOrDefaultAsync(cancellationToken);

            if (row is null || row.TotalCount == 0)
            {
                return MissionTelemetryTimeBounds.Empty;
            }

            return row;
        }

        private static ProjectionDefinition<TelemetryDataPoint> BuildFieldProjection(List<string> fields)
        {
            ProjectionDefinition<TelemetryDataPoint> projection = Builders<TelemetryDataPoint>.Projection
                .Include(x => x.TailId)
                .Include(x => x.Timestamp);

            foreach (string field in fields)
                projection = projection.Include(
                    $"{TelemetryRecordsConstants.Fields.TELEMETRY_DATA_PREFIX}.{field}");

            return projection;
        }
    }
}
