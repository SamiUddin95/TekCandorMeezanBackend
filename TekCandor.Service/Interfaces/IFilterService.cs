using TekCandor.Service.Models;
using System.Threading.Tasks;

namespace TekCandor.Service.Interfaces
{
    public interface IFilterService
    {
        Task<BranchFilterResponse> GetBranchFilterForUserAsync(long userId);
        Task<HubFilterResponse> GetHubFilterForUserAsync(long userId);
        Task<StatusFilterResponse> GetStatusFilterAsync();
        Task<InstrumentFilterResponse> GetInstrumentFilterAsync();
        Task<CycleFilterResponse> GetCycleFilterAsync();
        Task<ServiceRunFilterResponse> GetServiceRunFilterAsync();
    }
}
