using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Interfaces
{
    public interface IBusinessDateService
    {
        Task<BusinessDateDTO> CreateAsync(BusinessDateDTO dto);
        Task<List<BusinessDateDTO>> GetAllAsync();
    }
}
