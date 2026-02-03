using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface ICycleService
    {
        Task<PagedResult<CycleDTO>> GetAllCyclesAsync(int pageNumber, int pageSize);
        CycleDTO CreateCycle(CycleDTO cycle);
        CycleDTO? GetById(long id);
        CycleDTO? Update(CycleDTO cycle);
        bool SoftDelete(long id);
    }
}
