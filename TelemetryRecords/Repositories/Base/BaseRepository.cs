using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models.Config;
using TelemetryRecords.Repositories.Base.Interfaces;

namespace TelemetryRecords.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        protected abstract string CollectionName { get; }

        protected BaseRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfig)
        {
            IMongoDatabase database = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
            _collection = database.GetCollection<T>(CollectionName);
        }

        public virtual async Task<T?> GetLatestAsync(CancellationToken cancellationToken = default)
        {
            SortDefinition<T> sortByCreatedAtDesc = Builders<T>.Sort.Descending(TelemetryRecordsConstants.Fields.CREATED_AT);

            return await _collection
                .Find(FilterDefinition<T>.Empty)
                .Sort(sortByCreatedAtDesc)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            DateTime startOfDay = date.Date;
            DateTime endOfDay = startOfDay.AddDays(1);

            FilterDefinition<T> dateFilter = Builders<T>.Filter.And(
                Builders<T>.Filter.Gte(TelemetryRecordsConstants.Fields.CREATED_AT, startOfDay),
                Builders<T>.Filter.Lt(TelemetryRecordsConstants.Fields.CREATED_AT, endOfDay)
            );

            return await _collection.Find(dateFilter).ToListAsync(cancellationToken);
        }
    }
}
