using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChequeDepositController : ControllerBase
    {
        private readonly IChequeDepositImportService _importService;
        private readonly IImportHistoryService _importHistoryService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChequeDepositController> _logger;
        private readonly AppDbContext _context;
        public ChequeDepositController(
            IChequeDepositImportService importService,
            IImportHistoryService importHistoryService,
            IConfiguration configuration,
            ILogger<ChequeDepositController> logger,
            AppDbContext context)
        {
            _importService = importService;
            _importHistoryService = importHistoryService;
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportData()
        {
            try
            {
                var sftpHost = _configuration["SFTP:sftphostName"];
                var sftpPort = int.Parse(_configuration["SFTP:Port"] ?? "22");
                var sftpUser = _configuration["SFTP:UserName"];
                var sftpPassword = _configuration["SFTP:Password"];
                var remoteLocation = _configuration["SFTP:FileGetLocation"];
                var localLocation = _configuration["FileLocations:CLGFolderPath"];
                var callbackLimit = _context.Setting.Where(x => x.Name == "CallbackLimit").FirstOrDefault().Value;

                await _importService.ImportSFTPFilesAsync(
                    sftpHost, sftpPort, sftpUser, sftpPassword, remoteLocation, localLocation);

                var skippedFiles = await _importService.ProcessImportedFilesAsync(localLocation, callbackLimit);

                if (skippedFiles[0] == "File not Found")
                {
                    return BadRequest(ApiResponse<object>.Error("File Not Found", 200));
                }

                return Ok(ApiResponse<object>.Success(new { skippedFiles }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImportData");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("manual-import")]
        public async Task<IActionResult> ManualFileImport()
        {
            try
            {
                var manualImportPath = _configuration["FileLocations:ManualImportPath"];
                var processedPath = _configuration["FileLocations:Manualdelete"];
                var callbackLimit = _context.Setting.Where(x => x.Name == "CallbackLimit").FirstOrDefault().Value;

                if (string.IsNullOrEmpty(manualImportPath))
                    return BadRequest(ApiResponse<object>.Error("Manual import path not configured", 400));

                var skippedFiles = await _importService.ProcessManualImportAsync(manualImportPath, callbackLimit, processedPath);

               if (skippedFiles[0] == "File not Found")
                {
                    return BadRequest(ApiResponse<object>.Error("File Not Found", 200));
                }

                return Ok(ApiResponse<object>.Success(new { skippedFiles }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ManualFileImport");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpGet("import-history")]
        public async Task<IActionResult> ImportHistory([FromQuery] ImportHistoryRequest request)
        {
            try
            {
                var result = await _importHistoryService.GetImportHistoryAsync(request, HttpContext.RequestAborted);
                return Ok(ApiResponse<PagedResult<ImportHistoryResponse>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImportHistory");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpGet("manual-import-history")]
        public async Task<IActionResult> ManualImportHistory([FromQuery] ImportHistoryRequest request)
        {
            try
            {
                var result = await _importHistoryService.GetManualImportHistoryAsync(request, HttpContext.RequestAborted);
                return Ok(ApiResponse<PagedResult<ImportHistoryResponse>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ManualImportHistory");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}/details")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<ImportDataDetailResponse>>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImportDataDetail(
            long id,
            [FromQuery] ImportDataDetailRequest request,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest("id must be a positive number.");

            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var result = await _importHistoryService.GetImportDataDetailAsync(id, request, cancellationToken);

            if (result.TotalCount == 0)
                return NotFound(ApiResponse<object>.Error($"No detail records found for ImportDataId {id}.", 404));

            return Ok(ApiResponse<PagedResult<ImportDataDetailResponse>>.Success(result));
        }

        [HttpGet("{id:long}/manual-details")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<ImportDataDetailResponse>>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetManualImportDataDetail(
            long id,
            [FromQuery] ImportDataDetailRequest request,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<object>.Error("id must be a positive number.", 400));

            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest(ApiResponse<object>.Error("Page must be >= 1 and PageSize must be between 1 and 500.", 400));

            var result = await _importHistoryService.GetManualImportDataDetailAsync(id, request, cancellationToken);

            if (result.TotalCount == 0)
                return NotFound(ApiResponse<object>.Error($"No detail records found for Manual_ImportDataId {id}.", 404));

            return Ok(ApiResponse<PagedResult<ImportDataDetailResponse>>.Success(result));
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                string fileName = Path.GetFileName(file.FileName);

                string? folderPath = _configuration["FileLocations:ManualImportPath"];

                if (string.IsNullOrEmpty(folderPath))
                    return BadRequest("Upload path not configured.");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { message = "File Uploaded Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
