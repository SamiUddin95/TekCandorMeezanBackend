using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Interfaces
{
    public interface IChequeInfoService
    {
        Task<ChequeInfoDTO> CreateAsync(ChequeInfoDTO dto, string userId);
        Task<ChequeInfoDTO?> GetByIdAsync(long id);
        Task<List<ChequeInfoDTO>> GetAllAsync();
        Task<ChequeInfoDTO?> UpdateAsync(long id, ChequeInfoDTO dto, string userId);
        Task<bool> DeleteAsync(long id);
        Task<List<ChequeInfoDTO>> GetByBranchIdAsync(long branchId);
        Task<List<ChequeInfoDTO>> GetByStatusAsync(string status);
    }
}
