using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface ILiveMonitoringService
    {
        Task<List<LiveMonitoringDTO>> GetMonitoringDataAsync();
        Task<List<SignatureMonitoringDTO>> GetSignatureDataAsync();
    }
}