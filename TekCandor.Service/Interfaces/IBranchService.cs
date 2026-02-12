
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IBranchService
    {
        Task<PagedResult<BranchDTO>> GetAllBranchesAsync(int pageNumber, int pageSize, string? name = null);
        bool CreateBranch(BranchDTO branch);

        BranchDTO? GetById(long id);
        BranchDTO? Update(BranchDTO branch);
        bool SoftDelete(long id);
    }
}
