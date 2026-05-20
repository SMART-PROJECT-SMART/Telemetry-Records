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
        public async Task<ActionResult<MissionTelemetryPageRo>> GetByMission(
            [FromQuery] string missionId,
            [FromQuery] int tailId,
            [FromQuery] string? fields = null,
            [FromQuery] DateTime? startTime = null,
            [FromQuery] DateTime? endTime = null,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 0,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(missionId))
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.MISSION_ID_REQUIRED);
            }

            if (page < 0)
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.TELEMETRY_PAGE_INVALID);
            }

            if (pageSize < 0 || pageSize > TelemetryRecordsConstants.TelemetryQueryLimits.MaxPageSize)
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.TELEMETRY_PAGE_SIZE_INVALID);
            }

            List<string>? fieldList = !string.IsNullOrWhiteSpace(fields)
                ? fields.Split(
                    TelemetryRecordsConstants.TelemetryQuery.FieldListSeparator,
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                : null;

            MissionTelemetryPageRo pageResult = await _telemetryDataService.GetMissionTelemetryAsync(
                missionId,
                tailId,
                fieldList,
                startTime,
                endTime,
                page,
                pageSize,
                cancellationToken);

            return Ok(pageResult);
        }

        [HttpGet("bounds")]
        public async Task<ActionResult<MissionTelemetryBoundsRo>> GetBounds(
            [FromQuery] string missionId,
            [FromQuery] int tailId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(missionId))
            {
                return BadRequest(TelemetryRecordsConstants.ErrorMessages.MISSION_ID_REQUIRED);
            }

            MissionTelemetryBoundsRo bounds = await _telemetryDataService.GetMissionTelemetryBoundsAsync(
                missionId,
                tailId,
                cancellationToken);

            return Ok(bounds);
        }
    }
}
