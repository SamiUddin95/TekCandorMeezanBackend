using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Interfaces
{
    public interface IChequeInfoService
    {
        Task<ChequeInfoDTO> CreateAsync(ChequeInfoDTO dto, string userId);
        Task<ChequeInfoDTO?> GetByIdAsync(long id);
        Task<PagedResult<ChequeInfoDTO>> GetAllPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ChequeInfoDTO?> UpdateAsync(long id, ChequeInfoDTO dto, string userId);
        Task<bool> DeleteAsync(long id);
        Task<string> GenerateFileContentAsync(string receiverBranchCode, DateTime date);
        Task<List<ChequeInfoDTO>> GetByStatusAsync(string status, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PagedResult<ChequeInfoDTO>> GetSupervisorListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> ApproveAsync(long id, string userId);
        Task<bool> RejectAsync(long id, string userId, string remarks);
        Task<NiftUploadResultDTO> ProcessNiftFileAsync(string fileName, string fileContent, string fileType);
        Task<NiftUploadResultDTO> GetNiftUploadDataAsync(DateTime date);
        Task<bool> ForceMatchAsync(ForceMatchRequestDTO request);
        Task<PagedResult<ReturnListDTO>> GetReturnListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ReturnDetailDTO?> GetReturnDetailByIdAsync(long id);
        Task<PagedResult<FundRealizationDTO>> GetFundRealizationListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> MarkAsReturnAsync(long id, string userId);
        Task<BulkApproveResponseDTO> BulkSupervisorApproveAsync(BulkApproveRequestDTO request, string userId);
        Task<PagedResult<ChequeInfoDTO>> GetTransactionHistoryPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
    }
}
