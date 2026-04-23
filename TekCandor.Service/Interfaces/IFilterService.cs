using TekCandor.Service.Models;
using System.Threading.Tasks;

namespace TekCandor.Service.Interfaces
{
    public interface IFilterService
    {
        Task<BranchFilterResponse> GetBranchFilterForUserAsync(long userId);
        Task<BranchStatisticsDTO> GetBranchStatisticsAsync(string branchCode, DateTime? date = null);
        Task<HubFilterResponse> GetHubFilterForUserAsync(long userId);
        Task<HubStatisticsDTO> GetHubStatisticsAsync(string hubCode, DateTime? date = null);
        Task<StatusFilterResponse> GetStatusFilterAsync();
        Task<CbcStatusFilterResponse> GetCbcStatusFilterAsync();
        Task<InstrumentFilterResponse> GetInstrumentFilterAsync();
        Task<CycleFilterResponse> GetCycleFilterAsync();
        Task<ServiceRunFilterResponse> GetServiceRunFilterAsync();
        Task<ReturnReasonFilterResponse> GetReturnReasonFilterAsync();
        Task<BankFilterResponse> GetBankFilterAsync();
    }
}
