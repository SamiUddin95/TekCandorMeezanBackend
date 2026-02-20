using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IChequeDepositRepository
    {
        Task<bool> AddAsync(ChequedepositInfo chequeDeposit);
        Task<bool> AddRangeAsync(IEnumerable<ChequedepositInfo> chequeDeposits);
        Task<string> GetHubCodeAsync(string receiverBranchCode);
    }
}
