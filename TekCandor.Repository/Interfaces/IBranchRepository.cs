using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IBranchRepository
    {
        IEnumerable<Branch> GetAllQueryable();
        Task<Branch?> GetByIdAsync(long id);
        Task AddAsync(Branch branch);
        Task<bool> SaveChangesAsync();
    }

}
