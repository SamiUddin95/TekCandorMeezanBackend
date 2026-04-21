using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers.Outward
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/outward/[controller]")]
    public class ChequeInfoController : ControllerBase
    {
        private readonly IChequeInfoService _service;

        public ChequeInfoController(IChequeInfoService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ChequeInfoDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ChequeInfoDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";

                var result = await _service.CreateAsync(dto, userId);
                return Ok(ApiResponse<ChequeInfoDTO>.Success(result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(ApiResponse<object>.Success(new
                {
                    items = result,
                    totalCount = result.Count
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<ChequeInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Cheque info not found", 404));

                return Ok(ApiResponse<ChequeInfoDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<ChequeInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] ChequeInfoDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";

                var result = await _service.UpdateAsync(id, dto, userId);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Cheque info not found", 404));

                return Ok(ApiResponse<ChequeInfoDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Cheque info not found", 404));

                return Ok(ApiResponse<string>.Success("Cheque info deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }


        [HttpGet("supervisorList")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<ChequeInfoDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SupervisorList(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var result = await _service.GetSupervisorListPagedAsync(pageNumber, pageSize, fromDate, toDate);
                return Ok(ApiResponse<PagedResult<ChequeInfoDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("approve/{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Approve(long id)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _service.ApproveAsync(id, userId);
                
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Cheque not found", 404));

                return Ok(ApiResponse<string>.Success("Cheque approved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("bulk-supervisor-approve")]
        [ProducesResponseType(typeof(ApiResponse<BulkApproveResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BulkSupervisorApprove([FromBody] BulkApproveRequestDTO request)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _service.BulkSupervisorApproveAsync(request, userId);

                return Ok(ApiResponse<BulkApproveResponseDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("reject/{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reject(long id, string remarks)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _service.RejectAsync(id, userId, remarks);
                
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Cheque not found", 404));

                return Ok(ApiResponse<string>.Success("Cheque rejected successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("mark-return/{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsReturn(long id)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "system";
                var result = await _service.MarkAsReturnAsync(id, userId);
                
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Cheque not found", 404));

                return Ok(ApiResponse<string>.Success("Cheque marked as return successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("return-list")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReturnList()
        {
            try
            {
                var result = await _service.GetReturnListAsync();
                return Ok(ApiResponse<object>.Success(new
                {
                    items = result,
                    totalCount = result.Count
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("return-detail/{id:long}")]
        [ProducesResponseType(typeof(ApiResponse<ReturnDetailDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReturnDetail(long id)
        {
            try
            {
                var result = await _service.GetReturnDetailByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<string>.Error("Return detail not found", 404));

                return Ok(ApiResponse<ReturnDetailDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("fund-realization-list")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFundRealizationList()
        {
            try
            {
                var result = await _service.GetFundRealizationListAsync();
                return Ok(ApiResponse<object>.Success(new
                {
                    items = result,
                    totalCount = result.Count
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("reconcile-list")]
        [ProducesResponseType(typeof(ApiResponse<NiftUploadResultDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReconcileData([FromQuery] DateTime? date)
        {
            try
            {
                var searchDate = date ?? DateTime.Now.Date;
                var result = await _service.GetNiftUploadDataAsync(searchDate);

                return Ok(ApiResponse<NiftUploadResultDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("force-match")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForceMatch([FromBody] ForceMatchRequestDTO request)
        {
            try
            {
                if (request.NiftStagingId <= 0 || request.ChequeNo == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data", 400));

                var result = await _service.ForceMatchAsync(request);

                if (!result)
                    return NotFound(ApiResponse<string>.Error("Record not found or update failed", 404));

                return Ok(ApiResponse<string>.Success("Force match completed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("upload-nift")]
        [ProducesResponseType(typeof(ApiResponse<NiftUploadResultDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadNiftFile([FromForm] IFormFile file, [FromForm] string fileType)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(ApiResponse<string>.Error("File is required", 400));

                //if (string.IsNullOrEmpty(fileType) || (fileType.ToUpper() != "PAID" && fileType.ToUpper() != "RETURN"))
                    //return BadRequest(ApiResponse<string>.Error("File type must be 'PAID' or 'RETURN'", 400));

                if (!file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(ApiResponse<string>.Error("Only .txt files are allowed", 400));

                if (file.FileName.StartsWith("Paid", StringComparison.OrdinalIgnoreCase))
                {
                    fileType = "PAID";
                }
                else
                {
                    fileType = "RETURN";
                }
                string fileContent;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    fileContent = await reader.ReadToEndAsync();
                }

                var result = await _service.ProcessNiftFileAsync(file.FileName, fileContent, fileType);

                return Ok(ApiResponse<NiftUploadResultDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("generate-file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateFile([FromQuery] string receiverBranchCode, [FromQuery] DateTime date)
        {
            try
            {
                if (receiverBranchCode == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid branch ID", 400));

                var fileContent = await _service.GenerateFileContentAsync(receiverBranchCode, date);
                
                if (string.IsNullOrEmpty(fileContent))
                    return Ok(ApiResponse<string>.Success("No data found for the specified branch and date"));

                var bytes = Encoding.UTF8.GetBytes(fileContent);
                var fileName = $"ChequeInfo_{receiverBranchCode}_{date:dd-MM-yyyy}.txt";
                
                return File(bytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
