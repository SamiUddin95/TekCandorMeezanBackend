using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface IBusinessDateRepository
    {
        Task<BusinessDate> CreateAsync(BusinessDate businessDate);
        Task<List<BusinessDate>> GetAllAsync();
    }
}
