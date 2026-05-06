using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [AllowAnonymous]
        [HttpGet("status")]
        public async Task<IActionResult> GetLicenseStatus()
        {
            try
            {
                var status = await _licenseService.GetLicenseStatusAsync();
                return Ok(ApiResponse<LicenseStatusDTO>.Success(status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateLicense([FromBody] UpdateLicenseDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.EncryptedKey))
                    return BadRequest(ApiResponse<string>.Error("Encrypted key is required", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var saved = await _licenseService.UpdateLicenseAsync(dto.EncryptedKey, userId);

                if (!saved)
                    return BadRequest(ApiResponse<string>.Error("Invalid license key. The key could not be verified or saved.", 400));

                var newStatus = await _licenseService.GetLicenseStatusAsync();
                return Ok(ApiResponse<LicenseStatusDTO>.Success(newStatus));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
