using Microsoft.AspNetCore.Mvc;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models.Ro;
using TelemetryRecords.Services.TelemetryDataService.Interfaces;

namespace TelemetryRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryDataController : ControllerBase
    {
        private readonly ITelemetryDataService _telemetryDataService;

        public TelemetryDataController(ITelemetryDataService telemetryDataService)
        {
            _telemetryDataService = telemetryDataService;
        }

        [HttpGet("by-mission")]
        public async Task<ActionResult<List<MissionTelemetryRo>>> GetByMission(
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
