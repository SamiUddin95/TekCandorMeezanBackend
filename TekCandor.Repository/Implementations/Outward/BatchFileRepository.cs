using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;

namespace TekCandor.Repository.Implementations.Outward
{
    public class BatchFileRepository : IBatchFileRepository
    {
        private readonly AppDbContext _context;

        public BatchFileRepository(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Branch> GetBranchByNameAsync(string branchName)
        {
            return await _context.Branch
                .FirstOrDefaultAsync(b => b.Name == branchName && !b.IsDeleted);
        }

        public async Task SaveBatchAsync(Batch batch)
        {
            await _context.Batch.AddAsync(batch);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChequesAsync(List<ChequeInfo> cheques)
        {
            await _context.ChequeInfo.AddRangeAsync(cheques);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasBranchUploadedTodayAsync(string branchCode)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Batch
                .AnyAsync(b => b.Branch == branchCode
                    && b.CreatedAt >= today
                    && b.CreatedAt < tomorrow);
        }
    }
}
