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
    public class CycleController : ControllerBase
    {
        private readonly ICycleService _service;

        public CycleController(ICycleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var dtos = _service.GetAllCycles();
                return Ok(ApiResponse<IEnumerable<CycleDTO>>.Success(dtos));
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
                    return NotFound(ApiResponse<string>.Error("Cycle not found"));
                }
                return Ok(ApiResponse<CycleDTO>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CycleDTO dto)
        {
            try
            {
                var created = _service.CreateCycle(dto);
                return Ok(ApiResponse<CycleDTO>.Success(created, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CycleDTO dto)
        {
            try
            {
                dto.Id = id;
                var updated = _service.Update(dto);
                if (updated == null)
                {
                    return NotFound(ApiResponse<string>.Error("Cycle not found"));
                }
                return Ok(ApiResponse<CycleDTO>.Success(updated));
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
                    return NotFound(ApiResponse<string>.Error("Cycle not found"));
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
