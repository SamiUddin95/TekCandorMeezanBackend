using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;

        public FilterController(IFilterService filterService)
        {
            _filterService = filterService;
        }

        [HttpGet("branch")]
        public async Task<IActionResult> GetBranchFilter()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(ApiResponse<string>.Error("User not authenticated"));
                }

                var result = await _filterService.GetBranchFilterForUserAsync(userId);
                return Ok(ApiResponse<BranchFilterResponse>.Success(result));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("hub")]
        public async Task<IActionResult> GetHubFilter()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(ApiResponse<string>.Error("User not authenticated"));
                }

                var result = await _filterService.GetHubFilterForUserAsync(userId);
                return Ok(ApiResponse<HubFilterResponse>.Success(result));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatusFilter()
        {
            try
            {
                var result = await _filterService.GetStatusFilterAsync();
                return Ok(ApiResponse<StatusFilterResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("cbcStatus")]
        public async Task<IActionResult> GetCbcStatusFilter()
        {
            try
            {
                var result = await _filterService.GetCbcStatusFilterAsync();
                return Ok(ApiResponse<CbcStatusFilterResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("instrument")]
        public async Task<IActionResult> GetInstrumentFilter()
        {
            try
            {
                var result = await _filterService.GetInstrumentFilterAsync();
                return Ok(ApiResponse<InstrumentFilterResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("cycle")]
        public async Task<IActionResult> GetCycleFilter()
        {
            try
            {
                var result = await _filterService.GetCycleFilterAsync();
                return Ok(ApiResponse<CycleFilterResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("servicerun")]
        public async Task<IActionResult> GetServiceRunFilter()
        {
            try
            {
                var result = await _filterService.GetServiceRunFilterAsync();
                return Ok(ApiResponse<ServiceRunFilterResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("returnreason")]
        public async Task<IActionResult> GetReturnReasonFilter()
        {
            try
            {
                var result = await _filterService.GetReturnReasonFilterAsync();
                return Ok(ApiResponse<ReturnReasonFilterResponse>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
