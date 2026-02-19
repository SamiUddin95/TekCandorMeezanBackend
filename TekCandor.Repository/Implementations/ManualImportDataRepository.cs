using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ManualImportDataRepository : IManualImportDataRepository
    {
        private readonly AppDbContext _context;

        public ManualImportDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Manual_ImportData> GetByFileNameAsync(string fileName)
        {
            return await _context.Manual_ImportData
                .FirstOrDefaultAsync(x => x.FileName == fileName && !x.IsDeleted);
        }

        public async Task<long> AddAsync(Manual_ImportData importData)
        {
            _context.Manual_ImportData.Add(importData);
            await _context.SaveChangesAsync();
            return importData.Id;
        }

        public async Task<bool> AddDetailRangeAsync(IEnumerable<Manual_ImportDataDetails> details)
        {
            await _context.Manual_ImportDataDetails.AddRangeAsync(details);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStatisticsAsync(long importDataId, int successCount, int failureCount)
        {
            var importData = await _context.Manual_ImportData.FindAsync(importDataId);
            if (importData == null) return false;

            importData.SuccessfullRecords = successCount;
            importData.FailureRecords = failureCount;
            importData.UpdatedOn = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
