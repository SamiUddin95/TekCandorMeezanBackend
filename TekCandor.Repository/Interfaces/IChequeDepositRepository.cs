using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Models;

namespace TekCandor.Repository.Interfaces
{
    public interface IChequeDepositRepository
    {
        Task<bool> AddAsync(ChequedepositInfo chequeDeposit);
        Task<bool> AddRangeAsync(IEnumerable<ChequedepositInfo> chequeDeposits);
        Task<string> GetHubCodeAsync(string receiverBranchCode);

        //List
        Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetChequeDepositListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           string branchOrHub,
           string? hubIds,
           string? branchCodes,
           CancellationToken cancellationToken = default);
    }
}
