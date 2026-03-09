using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Models;
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
        private readonly IChequeDepositService _chequeDepositService;
        private readonly IImportHistoryService _importHistoryService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChequeDepositController> _logger;
        private readonly AppDbContext _context;
        private readonly ICoreBankingService _coreBankingService;
        public ChequeDepositController(
            IChequeDepositImportService importService,
            IChequeDepositService chequeDepositService,
            IImportHistoryService importHistoryService,
            IConfiguration configuration,
            ILogger<ChequeDepositController> logger,
            AppDbContext context,
            ICoreBankingService coreBankingService)
        {
            _importService = importService;
            _chequeDepositService = chequeDepositService;
            _importHistoryService = importHistoryService;
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _coreBankingService = coreBankingService;
        }

        #region Import Files
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
                var callbackLimit = _context.Setting.Where(x => x.Name == "generalsettings.callbackamount").FirstOrDefault().Value;

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
                var callbackLimit = _context.Setting.Where(x => x.Name == "generalsettings.callbackamount").FirstOrDefault().Value;

                if (string.IsNullOrEmpty(manualImportPath))
                    return BadRequest(ApiResponse<object>.Error("Manual import path not configured", 400));

                var skippedFiles = await _importService.ProcessManualImportAsync(manualImportPath, callbackLimit, processedPath);

                if (skippedFiles.Count > 0 && skippedFiles[0] == "File not Found")
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
            try
            {
                if (id <= 0)
                    return BadRequest(ApiResponse<object>.Error("id must be a positive number.", 400));

                if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                    return BadRequest(ApiResponse<object>.Error("Page must be >= 1 and PageSize must be between 1 and 500.", 400));

                var result = await _importHistoryService.GetManualImportDataDetailAsync(id, request, cancellationToken);

                //if (result.TotalCount == 0)
                //    return NotFound(ApiResponse<object>.Error($"No detail records found for Manual_ImportDataId {id}.", 404));

                return Ok(ApiResponse<PagedResult<ImportDataDetailResponse>>.Success(result));
            }
            catch (Exception)
            {
                return NotFound(ApiResponse<object>.Error($"No detail records found for Manual_ImportDataId {id}.", 404));

            }
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


        #endregion

        #region Cheque Deposit List

        [HttpGet("list")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(
           [FromQuery] ChequeDepositListRequestDTO request,
           CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetChequeDepositListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("callbackList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CallbackList(
           [FromQuery] ChequeDepositListRequestDTO request,
           CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetCallbackListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("ReturnList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ReturnList(
           [FromQuery] ChequeDepositListRequestDTO request,
           CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetReturnListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("BranchReturnList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BranchReturnList(
           [FromQuery] ChequeDepositListRequestDTO request,
           CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetBranchReturnListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("ApprovedList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ApprovedList(
          [FromQuery] ChequeDepositListRequestDTO request,
          CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetApprovedListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("UnAuthorizeList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnAuthorizeList(
    [FromQuery] ChequeDepositListRequestDTO request,
    CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetUnAuthorizedListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("RejectList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RejectList(
    [FromQuery] ChequeDepositListRequestDTO request,
    CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetRejectListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }


        [HttpGet("InProcessList")]
        [ProducesResponseType(typeof(PagedResult<ChequeDepositListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InProcessList(
   [FromQuery] ChequeDepositListRequestDTO request,
   CancellationToken cancellationToken)
        {
            if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 500)
                return BadRequest("Page must be >= 1 and PageSize must be between 1 and 500.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _chequeDepositService.GetInProcessListAsync(
                request, userId, cancellationToken);

            return Ok(result);
        }


        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _chequeDepositService.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id:long}/callback-edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CallBackEdit(long id)
        {
            try
            {
                var result = await _chequeDepositService.GetCallBackEditAsync(id);

                if (result == null)
                    return NotFound(ApiResponse<object>.Error("Cheque deposit not found", 404));

                return Ok(ApiResponse<ChequeDepositCallbackResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CallBackEdit for id: {Id}", id);
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}/branch-return-edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BranchReturnEdit(long id)
        {
            try
            {
                var result = await _chequeDepositService.GetBranchReturnEditAsync(id);

                if (result == null)
                    return NotFound(ApiResponse<object>.Error("Cheque deposit not found", 404));

                return Ok(ApiResponse<ChequeDepositBranchReturnResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BranchReturnEdit for id: {Id}", id);
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}/authorizer-edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AuthorizerEdit(long id)
        {
            try
            {
                var result = await _chequeDepositService.GetAuthorizerEditAsync(id);

                if (result == null)
                    return NotFound(ApiResponse<object>.Error("Cheque deposit not found", 404));

                return Ok(ApiResponse<ChequeDepositAuthorizerResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AuthorizerEdit for id: {Id}", id);
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}/system-reject-edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SystemRejectEdit(long id)
        {
            try
            {
                var result = await _chequeDepositService.GetRejectEditAsync(id);

                if (result == null)
                    return NotFound(ApiResponse<object>.Error("Cheque deposit not found", 404));

                return Ok(ApiResponse<ChequeDepositRejectResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RejectEdit for id: {Id}", id);
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("manual-start-service")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ManualStartService([FromQuery] bool isChecked = false)
        {
            try
            {
                await _coreBankingService.ImportRecordsAfterImportFileAsync();
                
                return Ok(ApiResponse<string>.Success("Services Run Successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ManualStartService");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("start-service")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> StartService([FromQuery] bool isChecked = false)
        {
            try
            {
                await _coreBankingService.ImportRecordsAfterImportFileAsync(isChecked);
                
                string message = isChecked 
                    ? "Send SMS and Services Run Successfully" 
                    : "Services Run Successfully";
                
                return Ok(ApiResponse<string>.Success(message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StartService");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("get-signatures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSign()
        {
            try
            {
                await _coreBankingService.GetSignaturesAsync();
                
                return Ok(ApiResponse<string>.Success("Signatures retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSign");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("get-signature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSignature([FromBody] GetSignatureRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.AccountNumber) || string.IsNullOrEmpty(request.ChequeNumber))
                {
                    return BadRequest(ApiResponse<object>.Error("Please fill all the required fields", 400));
                }

                var result = await _chequeDepositService.GetSignatureAsync(request.Id, request.AccountNumber, request.ChequeNumber);
                
                if (!result)
                {
                    return StatusCode(500, ApiResponse<object>.Error("SS Card Server Not Responding Please Retry"));
                }

                return Ok(ApiResponse<object>.Success(new { message = "Signature retrieved successfully", id = request.Id }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSignature");
                return StatusCode(500, ApiResponse<object>.Error("SS Card Server Not Responding Please Retry"));
            }
        }

        [HttpPost("pending-to-inprocess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PendingToInprocess([FromBody] PendingToInprocessRequest request)
        {
            try
            {
                if (request.SelectedIds == null || request.SelectedIds.Count == 0)
                {
                    return BadRequest(ApiResponse<object>.Error("No cheques selected", 400));
                }

                var result = await _chequeDepositService.PendingToInprocessAsync(request.SelectedIds);

                return Ok(ApiResponse<PendingToInprocessResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingToInprocess");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("pending-approve-selected")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PendingApproveSelected([FromBody] PendingApproveSelectedRequest request)
        {
            try
            {
                if (request.SelectedIds == null || request.SelectedIds.Count == 0)
                {
                    return BadRequest(ApiResponse<object>.Error("No cheques selected", 400));
                }

                // Get current user ID and login name from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loginName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                {
                    return Unauthorized(ApiResponse<object>.Error("User not authenticated", 401));
                }

                var result = await _chequeDepositService.PendingApproveSelectedAsync(request.SelectedIds, userId, loginName);

                return Ok(ApiResponse<PendingApproveSelectedResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingApproveSelected");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("pending-cheque-approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PendingChequeApprove([FromBody] PendingChequeApproveRequest request)
        {
            try
            {
                // Get current user ID and login name from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loginName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                {
                    return Unauthorized(ApiResponse<object>.Error("User not authenticated", 401));
                }

                var result = await _chequeDepositService.PendingChequeApproveAsync(
                    request.Id, 
                    request.AccountNumber, 
                    request.ChequeNumber, 
                    userId, 
                    loginName);

                if (result.Success)
                {
                    return Ok(ApiResponse<PendingChequeApproveResponse>.Success(result));
                }
                else
                {
                    return Ok(ApiResponse<PendingChequeApproveResponse>.Success(result));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingChequeApprove");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("pending-po-approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PendingPOApprove([FromBody] PendingPOApproveRequest request)
        {
            try
            {
                // Get current user ID and login name from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loginName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
                {
                    return Unauthorized(ApiResponse<object>.Error("User not authenticated", 401));
                }

                var result = await _chequeDepositService.PendingPOApproveAsync(request.Id, userId, loginName);

                return Ok(ApiResponse<PendingPOApproveResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PendingPOApprove");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        [HttpPost("import-images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportImages()
        {
            try
            {
                var sftpHost = _configuration["SFTP:sftphostName"];
                var sftpPort = int.Parse(_configuration["SFTP:Port"] ?? "22");
                var sftpUsername = _configuration["SFTP:sftpUserName"];
                var sftpPassword = _configuration["SFTP:sftpPassword"];
                var sftpImgFolder = _configuration["SFTP:sftpImgFolder"];
                var localPath = _configuration["FileLocations:NiftImages"];

                if (string.IsNullOrEmpty(sftpHost) || string.IsNullOrEmpty(sftpImgFolder) || string.IsNullOrEmpty(localPath))
                {
                    return BadRequest(ApiResponse<object>.Error("SFTP configuration is missing", 400));
                }

                // Ensure local directory exists
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                int downloadedCount = 0;
                int extractedCount = 0;

                using (var sftpClient = new Renci.SshNet.SftpClient(sftpHost, sftpPort, sftpUsername, sftpPassword))
                {
                    sftpClient.Connect();

                    var files = sftpClient.ListDirectory(sftpImgFolder);

                    foreach (var file in files)
                    {
                        if (file.Name == "." || file.Name == "..") continue;

                        string remoteFilePath = file.FullName;
                        string localFilePath = Path.Combine(localPath, file.Name);

                        // Process ZIP files
                        if (file.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            // Download the ZIP file
                            using (var fileStream = System.IO.File.Create(localFilePath))
                            {
                                sftpClient.DownloadFile(remoteFilePath, fileStream);
                            }
                            downloadedCount++;

                            // Extract the ZIP file
                            ExtractZipFile(localFilePath, localPath);
                            extractedCount++;

                            // Delete the ZIP file from SFTP after successful transfer
                            sftpClient.DeleteFile(remoteFilePath);

                            _logger.LogInformation("Downloaded and extracted ZIP file: {FileName}", file.Name);
                        }
                        // Process JPG files
                        else if (file.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || 
                                 file.Name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            // Download the JPG file
                            using (var fileStream = System.IO.File.Create(localFilePath))
                            {
                                sftpClient.DownloadFile(remoteFilePath, fileStream);
                            }
                            downloadedCount++;

                            // Delete the JPG file from SFTP after successful transfer
                            sftpClient.DeleteFile(remoteFilePath);

                            _logger.LogInformation("Downloaded JPG file: {FileName}", file.Name);
                        }
                    }

                    sftpClient.Disconnect();

                    _logger.LogInformation("Import Images completed. Downloaded: {Downloaded}, Extracted: {Extracted}", 
                        downloadedCount, extractedCount);
                }

                // Process images using the service
                var imageCount = await _importService.ProcessImagesAsync(localPath);

                return Ok(ApiResponse<object>.Success(new 
                { 
                    Message = "Images are Uploaded",
                    Downloaded = downloadedCount,
                    Extracted = extractedCount,
                    TotalImages = imageCount
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImportImages");
                return StatusCode(500, ApiResponse<object>.Error(ex.Message));
            }
        }

        private void ExtractZipFile(string zipFilePath, string extractPath)
        {
            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath, true);
                
                // Delete the ZIP file after extraction
                if (System.IO.File.Exists(zipFilePath))
                {
                    System.IO.File.Delete(zipFilePath);
                }

                _logger.LogInformation("Extracted ZIP file: {ZipFile}", zipFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting ZIP file: {ZipFile}", zipFilePath);
                throw;
            }
        }
    }

        #endregion
    }
