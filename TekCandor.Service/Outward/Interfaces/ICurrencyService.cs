using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<CurrencyDTO>> GetAllAsync();
    }
}
