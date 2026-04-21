using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces.Outward
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllAsync();
    }
}
