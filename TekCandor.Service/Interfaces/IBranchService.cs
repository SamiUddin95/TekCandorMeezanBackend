using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IBranchService
    {
        Task<PagedResult<BranchDTO>> GetBranchesAsync(int pageNumber, int pageSize, string? name = null);
        Task<BranchDTO?> GetByIdAsync(long id);
        Task<BranchDTO> CreateBranchAsync(BranchDTO branch);
        Task<BranchDTO?> UpdateBranchAsync(BranchDTO branch);
        Task<bool> SoftDeleteAsync(long id);


    }
}
