using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IReportService
    {
      
        Task<PagedResult<BranchWiseReportDTO>> GetBranchWiseReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branch);

        Task<PagedResult<CBCReportDTO>> GetCBCReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branch);

        Task<PagedResult<FinalReportDTO>> GetFinalReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branch, string? cycleCode);

        Task<PagedResult<ReturnMemoReportDTO>> GetReturnMemoReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branch);

        Task<PagedResult<ReturnRegisterDTO>> GetReturnRegisterReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branch, string? status, string? cycleCode);

        Task<PagedResult<ClearingLogReportDTO>> GetClearingLogReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? clearingCycle, string? hub);

    }
}
