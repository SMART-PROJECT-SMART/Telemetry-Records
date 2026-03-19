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
            DateTime startTime,
            DateTime endTime,
            CancellationToken cancellationToken = default)
        {
            FilterDefinition<TelemetryDataPoint> filter = Builders<TelemetryDataPoint>.Filter.And(
                Builders<TelemetryDataPoint>.Filter.Eq(x => x.TailId, tailId),
                Builders<TelemetryDataPoint>.Filter.Eq("TelemetryData.MissionId", missionIdHash),
                Builders<TelemetryDataPoint>.Filter.Gte(x => x.Timestamp, startTime),
                Builders<TelemetryDataPoint>.Filter.Lte(x => x.Timestamp, endTime)
            );

            SortDefinition<TelemetryDataPoint> sort =
                Builders<TelemetryDataPoint>.Sort.Ascending(x => x.Timestamp);

            return await _collection
                .Find(filter)
                .Sort(sort)
                .ToListAsync(cancellationToken);
        }
    }
}
