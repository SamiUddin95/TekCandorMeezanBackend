using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers.Outward
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/outward/[controller]")]
    public class DepositorTypeController : ControllerBase
    {
        private readonly IDepositorTypeService _service;

        public DepositorTypeController(IDepositorTypeService service)
        {
            _service = service;
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
