using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface IDepositorTypeRepository
    {
        Task<List<DepositorType>> GetAllAsync();
    }
}
