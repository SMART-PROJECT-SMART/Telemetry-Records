using Microsoft.AspNetCore.Mvc;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models.Ro;
using TelemetryRecords.Services.AssignmentService.Interfaces;

namespace TelemetryRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<AssignmentRo>> GetLatest(CancellationToken cancellationToken)
        {
            AssignmentRo? assignment = await _assignmentService.GetLatestAssignmentAsync(cancellationToken);
            if (assignment == null)
            {
                return NotFound(TelemetryRecordsConstants.ErrorMessages.ASSIGNMENT_NOT_FOUND);
            }
            return Ok(assignment);
        }

        [HttpGet("by-date/{date}")]
        public async Task<ActionResult<IEnumerable<AssignmentRo>>> GetByDate(string date, CancellationToken cancellationToken)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.INVALID_DATE_FORMAT);
            }

            IEnumerable<AssignmentRo> assignments = await _assignmentService.GetAssignmentsByDateAsync(parsedDate, cancellationToken);
            return Ok(assignments);
        }
    }
}
