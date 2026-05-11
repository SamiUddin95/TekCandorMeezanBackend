using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface IBatchFileRepository
    {
        Task<Branch> GetBranchByNameAsync(string branchName);
        Task SaveBatchAsync(Batch batch);
        Task SaveChequesAsync(List<ChequeInfo> cheques);
        Task<bool> HasBranchUploadedTodayAsync(string branchCode);
    }
}
