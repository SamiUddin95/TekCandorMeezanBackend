using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface IChequeInfoRepository
    {
        Task<ChequeInfo> CreateAsync(ChequeInfo chequeInfo);
        Task<ChequeInfo?> GetByIdAsync(long id);
        Task<(List<ChequeInfo> items, int totalCount)> GetAllPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ChequeInfo> UpdateAsync(ChequeInfo chequeInfo);
        Task<bool> DeleteAsync(long id);
        Task<List<ChequeInfo>> GetByBranchIdAndDateAsync(string receiverBranchCode, DateTime date);
        Task<List<ChequeInfo>> GetByHubcodeAndDateAsync(string hubcode, DateTime date);
        Task<List<ChequeInfo>> GetByStatusAsync(string status, DateTime? fromDate = null, DateTime? toDate = null);
        Task<(List<ChequeInfo> items, int totalCount)> GetByStatusPagedAsync(string status, int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> UpdateStatusAsync(long id, string status, string userId);
        Task<bool> UpdateRejectStatusAsync(long id, string status, string userId, string remarks);
        Task<ChequeInfo?> FindByChequeDetailsAsync(string chequeNo, decimal amount, string micr);
        Task<bool> UpdateMatchStatusAndStatusAsync(long id, string matchStatus, string status);
        Task<(List<object> items, int totalCount)> GetReturnListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<object?> GetReturnDetailByIdAsync(long id);
        Task<(List<object> items, int totalCount)> GetFundRealizationListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> MarkAsReturnAsync(long id, string userId);
        Task<int> BulkUpdateStatusAsync(List<long> ids, string status, string userId);
        Task<(List<ChequeInfo> items, int totalCount)> GetTransactionHistoryPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        Task<List<ChequeInfo>> GetByBatchIdAsync(string batchId);
    }
}
