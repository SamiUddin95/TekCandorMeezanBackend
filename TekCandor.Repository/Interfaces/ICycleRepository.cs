using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface ICycleRepository
    {
        IEnumerable<Cycle> GetAll();
        Cycle Add(Cycle cycle);
        Cycle? GetById(long id);
        Cycle? Update(Cycle cycle);
        bool SoftDelete(long id);
    }
}
