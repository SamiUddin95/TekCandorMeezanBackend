using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;

namespace TekCandor.Repository.Implementations.Outward
{
    public class BatchRepository : IBatchRepository
    {
        private readonly AppDbContext _context;

        public BatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Batch> CreateAsync(Batch batch)
        {
            await _context.Batch.AddAsync(batch);
            await _context.SaveChangesAsync();
            return batch;
        }

        public async Task<Batch?> GetByIdAsync(long id)
        {
            return await _context.Batch
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Batch?> GetByBatchIdAsync(string batchId)
        {
            return await _context.Batch
                .FirstOrDefaultAsync(b => b.BatchId == batchId);
        }

        public async Task<List<Batch>> GetAllAsync()
        {
            return await _context.Batch
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Batch?> UpdateAsync(Batch batch)
        {
            var existing = await _context.Batch.FindAsync(batch.Id);
            if (existing == null) return null;

            existing.Branch = batch.Branch;
            existing.TotalInstruments = batch.TotalInstruments;
            existing.TotalAmount = batch.TotalAmount;
            existing.UpdatedAt = batch.UpdatedAt;
            existing.UpdatedBy = batch.UpdatedBy;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var batch = await _context.Batch.FindAsync(id);
            if (batch == null) return false;

            _context.Batch.Remove(batch);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBatchTotalsAsync(string batchId)
        {
            var batch = await GetByBatchIdAsync(batchId);
            if (batch == null) return false;

            var totals = await _context.ChequeInfo
                .Where(c => c.BatchId == batchId)
                .GroupBy(c => c.BatchId)
                .Select(g => new
                {
                    TotalInstruments = g.Count(),
                    TotalAmount = g.Sum(c => c.Amount ?? 0)
                })
                .FirstOrDefaultAsync();

            if (totals != null)
            {
                batch.TotalInstruments = totals.TotalInstruments;
                batch.TotalAmount = totals.TotalAmount;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<Batch>> GetByStatusAsync(string status)
        {
            return await _context.Batch
                .Where(b => b.Status == status)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Batch>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.Batch
                .Where(b => b.CreatedAt >= fromDate && b.CreatedAt <= toDate)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

       
        public async Task<int> GetTotalBatchesTodayAsync(DateTime fromDate, DateTime toDate)
        {
            var start = fromDate.Date;
            var end = toDate.Date.AddDays(1);

            return await _context.Batch
                .Where(b => b.CreatedAt.HasValue &&
                            b.CreatedAt >= start &&
                            b.CreatedAt < end)
                .CountAsync();
        }
       
        public async Task<int> GetPendingAuthorizationCountAsync(DateTime fromDate, DateTime toDate)
        {
            var start = fromDate.Date;
            var end = toDate.Date.AddDays(1);

            return await _context.Batch
                .Where(b => b.CreatedAt.HasValue &&
                            b.CreatedAt >= start &&
                            b.CreatedAt < end &&
                            (b.Status == "Balanced" || b.Status == "Draft"))
                .CountAsync();
        }

        
        public async Task<decimal> GetAuthorizedValueAsync(DateTime fromDate, DateTime toDate)
        {
            var start = fromDate.Date;
            var end = toDate.Date.AddDays(1);

            return await _context.Batch
                .Where(b => b.CreatedAt.HasValue &&
                            b.CreatedAt >= start &&
                            b.CreatedAt < end &&
                            b.Status == "Authorized")
                .SumAsync(b => (decimal?)b.TotalAmount) ?? 0;
        }

        public async Task<int> GetProcessingExceptionsCountAsync(DateTime fromDate, DateTime toDate)
        {
            var start = fromDate.Date;
            var end = toDate.Date.AddDays(1);

            return await _context.Batch
                .Where(b => b.CreatedAt.HasValue &&
                            b.CreatedAt >= start &&
                            b.CreatedAt < end &&
                            (b.Status == "Exception" || b.Status == "Rejected"))
                .CountAsync();
        }
        public async Task<Batch?> GetByBranchAndDateAsync(string branch, DateTime fromDate, DateTime toDate)
        {
            return await _context.Batch
                .FirstOrDefaultAsync(b =>
                    b.Branch == branch &&
                    b.CreatedAt >= fromDate &&
                    b.CreatedAt < toDate
                );
        }
    }
}
