using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveMonitoringController : ControllerBase
    {
        private readonly ILiveMonitoringService _service;

        public LiveMonitoringController(ILiveMonitoringService service)
        {
            _service = service;
        }
        [HttpGet("monitoring")]
        public async Task<IActionResult> GetMonitoringData()
        {
            try
            {
                var monitoringData = await _service.GetMonitoringDataAsync();
                var signatureData = await _service.GetSignatureDataAsync();

                return Ok(ApiResponse<object>.Success(new
                {
                    Monitoring = monitoringData,
                    Signatures = signatureData
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}