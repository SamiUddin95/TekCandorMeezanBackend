using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers.Outward
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/outward/[controller]")]
    public class ChequeInfoController : ControllerBase
    {
        private readonly IChequeInfoService _service;

        public ChequeInfoController(IChequeInfoService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ChequeInfoDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ChequeInfoDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";

                var result = await _service.CreateAsync(dto, userId);
                return Ok(ApiResponse<ChequeInfoDTO>.Success(result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(ApiResponse<object>.Success(new
                {
                    items = result,
                    totalCount = result.Count
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<ChequeInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Cheque info not found", 404));

                return Ok(ApiResponse<ChequeInfoDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<ChequeInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] ChequeInfoDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";

                var result = await _service.UpdateAsync(id, dto, userId);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Cheque info not found", 404));

                return Ok(ApiResponse<ChequeInfoDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Cheque info not found", 404));

                return Ok(ApiResponse<string>.Success("Cheque info deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("branch/{branchId:long}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBranchId(long branchId)
        {
            try
            {
                var result = await _service.GetByBranchIdAsync(branchId);
                return Ok(ApiResponse<object>.Success(new
                {
                    items = result,
                    totalCount = result.Count
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStatus(string status)
        {
            try
            {
                var result = await _service.GetByStatusAsync(status);
                return Ok(ApiResponse<object>.Success(new
                {
                    items = result,
                    totalCount = result.Count
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
