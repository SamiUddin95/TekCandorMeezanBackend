using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {

        private readonly ISettingService _service;

        public SettingController(ISettingService service)
        {
            _service = service;
        }

        [HttpPut("UpdateCallbackAmount")]
        public IActionResult UpdateCallbackAmount(UpdateCallbackAmountDTO dto)
        {
            var updated = _service.UpdateCallbackAmount(dto);

            if (!updated)
                return NotFound(ApiResponse<string>.Error("Setting not found", 404));

            return Ok(ApiResponse<string>.Success("Callback amount updated successfully", 200));
        }
    }
}
