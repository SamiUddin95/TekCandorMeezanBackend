using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var hubIds = _httpContextAccessor.HttpContext?.User.FindFirst("HubIds")?.Value;
                var branchOrHub = _httpContextAccessor.HttpContext?.User.FindFirst("BranchOrHub")?.Value;

                if (string.IsNullOrEmpty(hubIds))
                    return Unauthorized("HubIds not found in token");

                var result = await _service.GetDashboardAsync(hubIds, branchOrHub);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
