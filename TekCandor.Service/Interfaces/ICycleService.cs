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
        IEnumerable<CycleDTO> GetAllCycles();
        CycleDTO CreateCycle(CycleDTO cycle);
        CycleDTO? GetById(Guid id);
        CycleDTO? Update(CycleDTO cycle);
        bool SoftDelete(Guid id);
    }
}
