using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IHubRepository
    {
       
        Task<IEnumerable<Hub>> GetAllQueryableAsync();
        Task<Hub?> GetByIdAsync(long id);
        Task AddAsync(Hub hub);
        Task<bool> SaveChangesAsync();


    }
}
