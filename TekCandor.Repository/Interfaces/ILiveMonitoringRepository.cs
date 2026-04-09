using System.Linq;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Implementations;

namespace TekCandor.Repository.Interfaces
{
    public interface ILiveMonitoringRepository
    {
        Task<IQueryable<ChequedepositInfo>> GetAllQueryableAsync();
    }
}