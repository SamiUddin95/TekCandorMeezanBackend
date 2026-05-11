using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers.Outward
{
    [Authorize]
    [Route("api/outward/[controller]")]
    [ApiController]
    public class BatchFileController : ControllerBase
    {
        private readonly IBatchFileService _batchFileService;

        public BatchFileController(IBatchFileService batchFileService)
        {
            _batchFileService = batchFileService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<FileUploadResultDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(ApiResponse<string>.Error("File is empty", 400));

                if (!file.FileName.EndsWith(".txt"))
                    return BadRequest(ApiResponse<string>.Error("Only .txt files allowed", 400));

                var uploadedBy = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _batchFileService.ProcessUploadedFileAsync(file, uploadedBy);

                return Ok(ApiResponse<FileUploadResultDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
