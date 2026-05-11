using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Interfaces
{
    public interface IBatchService
    {
        Task<BatchDTO> CreateBatchAsync(CreateBatchDTO dto, string userId);
        Task<BatchDTO?> GetBatchByIdAsync(long id);
        Task<BatchDTO?> GetBatchByBatchIdAsync(string batchId);
        Task<List<BatchDTO>> GetAllBatchesAsync();
        Task<List<BatchDTO>> GetBatchesByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<BatchDTO?> UpdateBatchAsync(long id, CreateBatchDTO dto, string userId);
        Task<bool> DeleteBatchAsync(long id);
        Task<bool> UpdateBatchTotalsAsync(string batchId);
        Task<BatchDTO?> SaveBatchAsDraftAsync(string batchId, string userId);
        Task<BatchDTO?> SubmitBatchForAuthorizationAsync(string batchId, string userId);
        Task<BatchDTO?> AuthorizeBatchAsync(string batchId, string userId);
        Task<BatchDTO?> RejectBatchAsync(string batchId, string userId, string rejectionReason);
        
        Task<BatchStatisticsDTO> GetBatchStatisticsAsync(DateTime fromDate, DateTime toDate);
        Task<BatchDateRangeWithStatsDTO> GetBatchesByDateRangeWithStatsAsync(DateTime fromDate, DateTime toDate);
        Task<BatchWithInstrumentsDTO?> GetBatchWithInstrumentsAsync(string batchId);
        Task<List<ChequeInfoDTO>> GetInstrumentsByBatchIdAsync(string batchId);
        Task<bool> ApproveBatchAsync(string batchId, string userId);
        Task<bool> ApproveInstrumentAsync(long id, string userId);


    }
}
