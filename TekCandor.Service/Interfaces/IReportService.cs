using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IReportService
    {
      
        Task<PagedResult<BranchWiseReportDTO>> GetBranchWiseReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? chequeNumber, string? accountNumber, string? hubCode, string? status);

        Task<PagedResult<CBCReportDTO>> GetCBCReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? accountNumber, string? status, string? hub);

        Task<PagedResult<FinalReportDTO>> GetFinalReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? cycleCode);

        Task<PagedResult<ReturnMemoReportDTO>> GetReturnMemoReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? chequenumber,string? accountnumber);

        Task<PagedResult<ReturnRegisterDTO>> GetReturnRegisterReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? chequeNumber, string? accountNumber, string? hubCode, string? status);

        Task<PagedResult<ClearingLogReportDTO>> GetClearingLogReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? clearingCycle, string? hub);

        Task<PagedResult<InwardClearingReportDTO>> GetInwardClearingReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? status, string? ReceiverbranchCodeBranchCode, string? hub);

    }
}
