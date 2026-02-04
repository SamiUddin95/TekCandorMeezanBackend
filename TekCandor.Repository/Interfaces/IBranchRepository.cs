using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IBranchRepository
    {
        Task<IQueryable<Branch>> GetAllQueryableAsync();

        Task<Branch?> GetByIdAsync(long id);
        //Task<Branch?> GetByIdAsyncupd(long id);

        Task AddAsync(Branch branch);
        Task<bool> SaveChangesAsync();
    }

}
