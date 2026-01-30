using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;
        public BranchController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]

        public IActionResult Get()
        {
            try
            {
                var dtos = _service.GetBranches();
                return Ok(ApiResponse<IEnumerable<BranchDTO>>.Success(dtos));
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
                var created = _service.CreateBranch(dto);
                return Ok(ApiResponse<BranchDTO>.Success(created, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
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

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] BranchDTO dto)
        {
            try
            {
                var updated = _service.UpdateBranch(dto);
                return Ok(ApiResponse<BranchDTO>.Success(updated, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpDelete]
        [Route("{id}")]

        public IActionResult Delete(Guid id)
        {
            try
            {
                var deleted = _service.SoftDelete(id);
                return Ok(ApiResponse<bool>.Success(deleted, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

       
    }

}
