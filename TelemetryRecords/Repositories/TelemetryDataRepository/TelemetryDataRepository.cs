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

        public async Task<List<TelemetryDataPoint>> GetByMissionAsync(
            double missionIdHash,
            int tailId,
            List<string>? fields = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken cancellationToken = default)
        {
            var filters = new List<FilterDefinition<TelemetryDataPoint>>
            {
                Builders<TelemetryDataPoint>.Filter.Eq(x => x.TailId, tailId),
                Builders<TelemetryDataPoint>.Filter.Eq("TelemetryData.MissionId", missionIdHash)
            };

            if (startTime.HasValue)
                filters.Add(Builders<TelemetryDataPoint>.Filter.Gte(x => x.Timestamp, startTime.Value));

            if (endTime.HasValue)
                filters.Add(Builders<TelemetryDataPoint>.Filter.Lt(x => x.Timestamp, endTime.Value));

            FilterDefinition<TelemetryDataPoint> filter = Builders<TelemetryDataPoint>.Filter.And(filters);

            SortDefinition<TelemetryDataPoint> sort =
                Builders<TelemetryDataPoint>.Sort.Ascending(x => x.Timestamp);

            IFindFluent<TelemetryDataPoint, TelemetryDataPoint> query = _collection.Find(filter).Sort(sort);

            if (fields is { Count: > 0 })
            {
                ProjectionDefinitionBuilder<TelemetryDataPoint> projBuilder = Builders<TelemetryDataPoint>.Projection;
                ProjectionDefinition<TelemetryDataPoint> projection = projBuilder
                    .Include(x => x.TailId)
                    .Include(x => x.Timestamp);

                foreach (string field in fields)
                    projection = projection.Include($"TelemetryData.{field}");

                query = query.Project<TelemetryDataPoint>(projection);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
