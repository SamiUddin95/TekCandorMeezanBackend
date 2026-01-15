using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IBranchService
    {
        IEnumerable<BranchDTO> GetBranches();
        BranchDTO CreateBranch(BranchDTO branch);
        
        BranchDTO? UpdateBranch(BranchDTO branch);
        bool SoftDelete(Guid id);

        BranchDTO? GetById(Guid Id);
    }
}
