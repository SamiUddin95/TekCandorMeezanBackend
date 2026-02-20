using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize);

            return Ok(ApiResponse<object>.Success(new
            {
                items = result.Items,
                totalCount = result.TotalCount,
                pageNumber = result.PageNumber,
                pageSize = result.PageSize,
                totalPages = result.TotalPages
            }, 200));
        }
    }
}
