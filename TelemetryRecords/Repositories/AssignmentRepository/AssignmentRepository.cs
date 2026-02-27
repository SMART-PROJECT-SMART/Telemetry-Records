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
    }
}
