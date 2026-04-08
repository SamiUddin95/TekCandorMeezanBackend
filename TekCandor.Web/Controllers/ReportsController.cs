using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }


        [HttpGet("BranchWiseReport")]
        public async Task<IActionResult> BranchWiseReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branchCode = null)

        {
            try
            {
                var result = await _service.GetBranchWiseReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branchCode);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("CBCReport")]
        public async Task<IActionResult> CBCReport(int pageNumber = 1,int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branchCode = null, string? accountNumber = null, string? status = null, string? hub = null)

        {
            try
            {
                var result = await _service.GetCBCReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branchCode, accountNumber,status,hub);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("FinalReport")]
        public async Task<IActionResult> FinalReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branchCode = null, string? cycleCode = null)

        {
            try
            {
                var result = await _service.GetFinalReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branchCode, cycleCode);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("ReturnMemoReport")]
        public async Task<IActionResult> ReturnMemoReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branchCode = null,string? chequeNumber = null,string? accountnumber=null )

        {
            try
            {
                var result = await _service.GetReturnMemoReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branchCode, chequeNumber,accountnumber);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        
        [HttpGet("ReturnRegisterReport")]
        public async Task<IActionResult> ReturnRegisterReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branchCode = null, string? status = null, string? cycleCode = null)

        {
            try
            {
                var result = await _service.GetReturnRegisterReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branchCode, status, cycleCode);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        
        [HttpGet("ClearingLogReport")]
        public async Task<IActionResult> ClearingLogReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? clearingCycle = null, string? hub = null)

        {
            try
            {
                var result = await _service.GetClearingLogReportAsync(
                    pageNumber, pageSize, fromDate, toDate, clearingCycle, hub);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("InwardClearingReport")]
        public async Task<IActionResult> InwardClearingReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? status = null, string? branchCode = null, string? hub = null)

        {
            try
            {
                var result = await _service.GetInwardClearingReportAsync(
                    pageNumber, pageSize, fromDate, toDate, status, branchCode, hub);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

    }
}
