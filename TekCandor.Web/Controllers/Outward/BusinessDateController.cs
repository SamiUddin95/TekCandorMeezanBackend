using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers.Outward
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/outward/[controller]")]
    public class BusinessDateController : ControllerBase
    {
        private readonly IBusinessDateService _service;

        public BusinessDateController(IBusinessDateService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BusinessDateDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] BusinessDateDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var result = await _service.CreateAsync(dto);
                return Ok(ApiResponse<BusinessDateDTO>.Success(result, 201));
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

    }
}
