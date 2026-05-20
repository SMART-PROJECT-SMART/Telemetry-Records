using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models;
using TelemetryRecords.Models.Config;
using TelemetryRecords.Repositories.AssignmentRepository.Interfaces;
using TelemetryRecords.Repositories.Base;

namespace TelemetryRecords.Repositories.AssignmentRepository
{
    public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
    {
        protected override string CollectionName => TelemetryRecordsConstants.Collections.ASSIGNMENTS_COLLECTION;
        protected override string TimestampFieldName => TelemetryRecordsConstants.Fields.CREATED_AT;

        public AssignmentRepository(IMongoClient mongoClient, IOptions<MongoDbConfiguration> mongoDbConfig)
            : base(mongoClient, mongoDbConfig)
        {
        }

        public async Task<Assignment?> FindLatestContainingMissionIdAsync(
            string missionId, CancellationToken cancellationToken = default)
        {
            FilterDefinition<Assignment> suggestedMatch =
                Builders<Assignment>.Filter.ElemMatch(
                    a => a.SuggestedAssignments,
                    Builders<MissionToUavAssignment>.Filter.Eq(m => m.Mission.Id, missionId));

            FilterDefinition<Assignment> actualMatch =
                Builders<Assignment>.Filter.ElemMatch(
                    a => a.ActualAssignments,
                    Builders<MissionToUavAssignment>.Filter.Eq(m => m.Mission.Id, missionId));

            FilterDefinition<Assignment> filter =
                Builders<Assignment>.Filter.Or(suggestedMatch, actualMatch);

            SortDefinition<Assignment> sort =
                Builders<Assignment>.Sort.Descending(TimestampFieldName);

            return await _collection
                .Find(filter)
                .Sort(sort)
                .Limit(1)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
