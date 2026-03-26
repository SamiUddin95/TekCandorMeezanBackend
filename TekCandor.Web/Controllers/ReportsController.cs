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
        public async Task<IActionResult> BranchWiseReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branch = null)

        {
            try
            {
                var result = await _service.GetBranchWiseReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branch);

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
        public async Task<IActionResult> CBCReport(int pageNumber = 1,int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branch = null)

        {
            try
            {
                var result = await _service.GetCBCReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branch);

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
        public async Task<IActionResult> FinalReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branch = null, string? cycleCode = null)

        {
            try
            {
                var result = await _service.GetFinalReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branch, cycleCode);

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
        public async Task<IActionResult> ReturnMemoReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branch = null)

        {
            try
            {
                var result = await _service.GetReturnMemoReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branch);

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
        public async Task<IActionResult> ReturnRegisterReport(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? branch = null, string? status = null, string? cycleCode = null)

        {
            try
            {
                var result = await _service.GetReturnRegisterReportAsync(
                    pageNumber, pageSize, fromDate, toDate, branch, status, cycleCode);

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


    }
}
