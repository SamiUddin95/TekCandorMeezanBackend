using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IQueryable<ChequedepositInfo>> GetAllQueryableAsync();

        Task<IQueryable<Cycle>> GetAllQueryableCycleAsync();


    }
}
