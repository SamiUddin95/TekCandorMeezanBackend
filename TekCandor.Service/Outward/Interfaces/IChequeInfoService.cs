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
        Task<List<ChequeInfoDTO>> GetAllAsync();
        Task<ChequeInfoDTO?> UpdateAsync(long id, ChequeInfoDTO dto, string userId);
        Task<bool> DeleteAsync(long id);
        Task<string> GenerateFileContentAsync(string receiverBranchCode, DateTime date);
        Task<List<ChequeInfoDTO>> GetByStatusAsync(string status);
        Task<bool> ApproveAsync(long id, string userId);
        Task<bool> RejectAsync(long id, string userId);
    }
}
