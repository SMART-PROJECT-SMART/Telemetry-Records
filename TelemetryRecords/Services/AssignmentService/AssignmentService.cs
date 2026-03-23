using TelemetryRecords.Extensions;
using TelemetryRecords.Models;
using TelemetryRecords.Models.Ro;
using TelemetryRecords.Repositories.AssignmentRepository.Interfaces;
using TelemetryRecords.Services.AssignmentService.Interfaces;

namespace TelemetryRecords.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<AssignmentRo?> GetLatestAssignmentAsync(CancellationToken cancellationToken = default)
        {
            Assignment? assignment = await _assignmentRepository.GetLatestAsync(cancellationToken);
            return assignment?.ToRo();
        }

        public async Task<IEnumerable<AssignmentRo>> GetAssignmentsByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            IEnumerable<Assignment> assignments = await _assignmentRepository.GetByDateAsync(date, cancellationToken);
            return assignments.ToRo();
        }

        public async Task<MissionRo?> GetMissionByIdAsync(string missionId, CancellationToken cancellationToken = default)
        {
            Assignment? assignment = await _assignmentRepository.FindLatestContainingMissionIdAsync(missionId, cancellationToken);
            if (assignment == null) return null;

            MissionToUavAssignment? match =
                assignment.ActualAssignments.FirstOrDefault(a => a.Mission.Id == missionId)
                ?? assignment.SuggestedAssignments.FirstOrDefault(a => a.Mission.Id == missionId);

            return match?.Mission.ToRo();
        }
    }
}
