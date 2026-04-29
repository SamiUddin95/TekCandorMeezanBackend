using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface IBatchRepository
    {
        Task<Batch> CreateAsync(Batch batch);
        Task<Batch?> GetByIdAsync(long id);
        Task<Batch?> GetByBatchIdAsync(string batchId);
        Task<List<Batch>> GetAllAsync();
        Task<List<Batch>> GetByStatusAsync(string status);
        Task<List<Batch>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<Batch?> UpdateAsync(Batch batch);
        Task<bool> DeleteAsync(long id);
        Task<bool> UpdateBatchTotalsAsync(string batchId);
        Task<int> GetTotalBatchesTodayAsync();
        Task<int> GetPendingAuthorizationCountAsync();
        Task<decimal> GetAuthorizedValueAsync();
        Task<int> GetProcessingExceptionsCountAsync();
    }
}
