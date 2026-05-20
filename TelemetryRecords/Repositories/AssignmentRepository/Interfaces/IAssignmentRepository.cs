using TelemetryRecords.Models;
using TelemetryRecords.Repositories.Base.Interfaces;

namespace TelemetryRecords.Repositories.AssignmentRepository.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<Assignment?> FindLatestContainingMissionIdAsync(string missionId, CancellationToken cancellationToken = default);
    }
}
