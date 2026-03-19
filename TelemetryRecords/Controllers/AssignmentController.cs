using Microsoft.AspNetCore.Mvc;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models.Ro;
using TelemetryRecords.Services.AssignmentService.Interfaces;
using TelemetryRecords.Services.TelemetryDataService.Interfaces;

namespace TelemetryRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentRecordsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ITelemetryDataService _telemetryDataService;

        public AssignmentRecordsController(
            IAssignmentService assignmentService,
            ITelemetryDataService telemetryDataService)
        {
            _assignmentService = assignmentService;
            _telemetryDataService = telemetryDataService;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<AssignmentRo>> GetLatest(CancellationToken cancellationToken)
        {
            AssignmentRo? assignment = await _assignmentService.GetLatestAssignmentAsync(
                cancellationToken
            );
            if (assignment == null)
            {
                return NotFound(TelemetryRecordsConstants.ErrorMessages.ASSIGNMENT_NOT_FOUND);
            }
            return Ok(assignment);
        }

        [HttpGet("by-date/{date}")]
        public async Task<ActionResult<IEnumerable<AssignmentRo>>> GetByDate(
            string date,
            CancellationToken cancellationToken
        )
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.INVALID_DATE_FORMAT);
            }

            IEnumerable<AssignmentRo> assignments =
                await _assignmentService.GetAssignmentsByDateAsync(parsedDate, cancellationToken);
            return Ok(assignments);
        }

        [HttpGet("telemetry")]
        public async Task<ActionResult<List<MissionTelemetryRo>>> GetMissionTelemetry(
            [FromQuery] string missionId,
            [FromQuery] int tailId,
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(missionId))
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.MISSION_ID_REQUIRED);
            }

            List<MissionTelemetryRo> telemetry = await _telemetryDataService.GetMissionTelemetryAsync(
                missionId, tailId, startTime, endTime, cancellationToken);

            return Ok(telemetry);
        }
    }
}
