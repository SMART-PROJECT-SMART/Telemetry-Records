using TelemetryRecords.Models.Ro;

namespace TelemetryRecords.Services.AssignmentService.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentRo?> GetLatestAssignmentAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<AssignmentRo>> GetAssignmentsByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    }
}
