using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers.Outward
{
    [Authorize]
    [Route("api/outward/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBatch([FromBody] CreateBatchDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";

                var result = await _batchService.CreateBatchAsync(dto, userId);
                return Ok(ApiResponse<BatchDTO>.Success(result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<BatchDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBatches()
        {
            try
            {
                var result = await _batchService.GetAllBatchesAsync();
                return Ok(ApiResponse<List<BatchDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBatchById(long id)
        {
            try
            {
                var result = await _batchService.GetBatchByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("by-batch-id/{batchId}")]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBatchByBatchId(string batchId)
        {
            try
            {
                var result = await _batchService.GetBatchByBatchIdAsync(batchId);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBatch(long id, [FromBody] CreateBatchDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";

                var result = await _batchService.UpdateBatchAsync(id, dto, userId);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBatch(long id)
        {
            try
            {
                var result = await _batchService.DeleteBatchAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<string>.Success("Batch deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("update-totals/{batchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBatchTotals(string batchId)
        {
            try
            {
                var result = await _batchService.UpdateBatchTotalsAsync(batchId);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<string>.Success("Batch totals updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("submit/{batchId}")]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmitBatchForAuthorization(string batchId)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _batchService.SubmitBatchForAuthorizationAsync(batchId, userId);
                
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchDTO>.Success(result));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("authorize/{batchId}")]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AuthorizeBatch(string batchId)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _batchService.AuthorizeBatchAsync(batchId, userId);
                
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchDTO>.Success(result));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("reject/{batchId}")]
        [ProducesResponseType(typeof(ApiResponse<BatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectBatch(string batchId, [FromBody] RejectBatchDTO dto)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _batchService.RejectBatchAsync(batchId, userId, dto.RejectionReason);
                
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchDTO>.Success(result));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("statistics")]
        [ProducesResponseType(typeof(ApiResponse<BatchStatisticsDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBatchStatistics()
        {
            try
            {
                var result = await _batchService.GetBatchStatisticsAsync();
                return Ok(ApiResponse<BatchStatisticsDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{batchId}/instruments")]
        [ProducesResponseType(typeof(ApiResponse<BatchWithInstrumentsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBatchWithInstruments(string batchId)
        {
            try
            {
                var result = await _batchService.GetBatchWithInstrumentsAsync(batchId);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Batch not found", 404));

                return Ok(ApiResponse<BatchWithInstrumentsDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("date-range")]
        [ProducesResponseType(typeof(ApiResponse<List<BatchDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBatchesByDateRange([FromQuery] string fromDate, [FromQuery] string toDate)
        {
            try
            {
                if (!DateTime.TryParse(fromDate, out var parsedFromDate) || !DateTime.TryParse(toDate, out var parsedToDate))
                {
                    return BadRequest(ApiResponse<string>.Error("Invalid date format", 400));
                }

                var result = await _batchService.GetBatchesByDateRangeAsync(parsedFromDate, parsedToDate);
                return Ok(ApiResponse<List<BatchDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
