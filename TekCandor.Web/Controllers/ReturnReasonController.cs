using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnReasonController : ControllerBase
    {
        private readonly IReturnReasonService _service;
        public ReturnReasonController(IReturnReasonService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var dtos = _service.GetAll();
                return Ok(ApiResponse<IEnumerable<ReturnReasonDTO>>.Success(dtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var dto = _service.GetById(id);
                if (dto == null) return NotFound(ApiResponse<string>.Error("ReturnReason not found"));
                return Ok(ApiResponse<ReturnReasonDTO>.Success(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ReturnReasonDTO dto)
        {
            try
            {
                var created = _service.Create(dto);
                return Ok(ApiResponse<ReturnReasonDTO>.Success(created, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] ReturnReasonDTO dto)
        {
            try
            {
                var updated = _service.Update(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("ReturnReason not found"));
                return Ok(ApiResponse<ReturnReasonDTO>.Success(updated, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
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
