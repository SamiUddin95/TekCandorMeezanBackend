using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class HubController : ControllerBase
    {
        private readonly IHubService _service;

        public HubController(IHubService service)
        {
            _service = service;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10,string? name = null)
        {
            try
            {
                var result = await _service.GetAllHubsAsync(pageNumber, pageSize,name);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                    return NotFound(ApiResponse<string>.Error("Hub not found"));

                return Ok(ApiResponse<HubDTO>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HubDTO dto)
        {
            try
            {
                var created = await _service.CreateHubAsync(dto);
                return Ok(ApiResponse<HubDTO>.Success(created, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] HubDTO dto)
        {
            try
            {
                dto.Id = id;
                var updated = await _service.UpdateAsync(dto);
                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Hub not found"));

                return Ok(ApiResponse<HubDTO>.Success(updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var ok = await _service.SoftDeleteAsync(id);
                if (!ok)
                    return NotFound(ApiResponse<string>.Error("Hub not found"));

                return Ok(ApiResponse<string>.Success("Deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

    }
}
