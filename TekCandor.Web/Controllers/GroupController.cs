using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Authorization;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _service;

        public GroupController(IGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAllGroupsAsync(pageNumber, pageSize);
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
                if (dto == null) return NotFound(ApiResponse<string>.Error("Group not found"));

                return Ok(ApiResponse<GroupDTO>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupDTO dto)
        {
            try
            {
                var created = await _service.CreateGroupAsync(dto);
                return Ok(ApiResponse<GroupDTO>.Success(created, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] GroupDTO dto)
        {
            try
            {
                dto.Id = id;
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("Group not found"));

                return Ok(ApiResponse<GroupDTO>.Success(updated));
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
                if (!ok) return NotFound(ApiResponse<string>.Error("Group not found"));

                return Ok(ApiResponse<string>.Success("Deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("assign-permissions")]
        [HasPermission("Security.Groups")]
        public async Task<IActionResult> AssignPermissions([FromBody] AssignPermissionsDTO dto)
        {
            await _service.AssignPermissionsAsync(dto);

            return Ok("Permissions assigned successfully.");
        }

    }
}
