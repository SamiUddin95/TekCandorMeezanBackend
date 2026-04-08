using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IReportRepository
    {
        Task<IQueryable<ChequedepositInfo>> GetChequeQueryableAsync();
        //Task<IQueryable<Cycle>> GetCycleQueryableAsync();

        Task<IQueryable<ReturnReason>> GetReturnReasonQueryableAsync();
        Task<string?> GetSettingValueAsync(string name);
        Task<IQueryable<Branch>> GetBranchQueryableAsync();
        Task<IQueryable<Cycle>> GetCycleQueryableAsync();
        Task<IQueryable<Bank>> GetBankQueryableAsync();



    }
}
