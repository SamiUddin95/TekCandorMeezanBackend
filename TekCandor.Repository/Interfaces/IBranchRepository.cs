using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IBranchRepository
    {
        IEnumerable<Branch> GetAll();
        Branch Add(Branch branch);
        Branch? GetById(Guid id);
        Branch? Update(Branch branch);
        bool SoftDelete(Guid id);


    }
}
