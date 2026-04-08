using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<ChequedepositInfo>> GetChequeQueryableAsync()
        {
            return _context.chequedepositInformation.AsNoTracking();
        }
        public async Task<IQueryable<ReturnReason>> GetReturnReasonQueryableAsync()
        {
            return _context.ReturnReason.AsNoTracking();
        }
       


        public async Task<string?> GetSettingValueAsync(string name)
        {
            return await _context.Setting
                .Where(x => x.Name == name)
                .Select(x => x.Value)
                .FirstOrDefaultAsync();
        }

        public async Task<IQueryable<Branch>> GetBranchQueryableAsync()
        {
            return _context.Branch.AsNoTracking();
        }

        public async Task<IQueryable<Cycle>> GetCycleQueryableAsync()
        {
            return _context.Cycles.AsNoTracking();
        }

        public async Task<IQueryable<Bank>> GetBankQueryableAsync()
        {
            return _context.Bank.AsNoTracking();
        }
    }
}