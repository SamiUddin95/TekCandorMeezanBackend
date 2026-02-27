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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            try
            {
                var result = await _service.GetAllBranchesAsync(pageNumber, pageSize, name);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var dto = _service.GetById(id);
                if (dto == null)
                {
                    return NotFound(ApiResponse<string>.Error("Branch not found"));
                }
                return Ok(ApiResponse<BranchDTO>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] BranchDTO dto)
        {
            try
            {
                _service.CreateBranch(dto);

                return Ok(
                    ApiResponse<string>.Success("Branch successfully created", 200)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }


        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] BranchDTO dto)
        {
            try
            {
                dto.Id = id;
                var updated = _service.Update(dto);
                if (updated == null)
                {
                    return NotFound(ApiResponse<string>.Error("Branch not found"));
                }
                return Ok(ApiResponse<BranchDTO>.Success(updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var ok = _service.SoftDelete(id);
                if (!ok)
                {
                    return NotFound(ApiResponse<string>.Error("Branch not found"));
                }
                return Ok(ApiResponse<string>.Success("Deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}

