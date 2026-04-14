using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface IChequeInfoRepository
    {
        Task<ChequeInfo> CreateAsync(ChequeInfo chequeInfo);
        Task<ChequeInfo?> GetByIdAsync(long id);
        Task<List<ChequeInfo>> GetAllAsync();
        Task<ChequeInfo> UpdateAsync(ChequeInfo chequeInfo);
        Task<bool> DeleteAsync(long id);
        Task<List<ChequeInfo>> GetByBranchIdAsync(long branchId);
        Task<List<ChequeInfo>> GetByStatusAsync(string status);
    }
}
