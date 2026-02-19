using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class ImportDataRepository : IImportDataRepository
    {
        private readonly AppDbContext _context;

        public ImportDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ImportData> GetByFileNameAsync(string fileName)
        {
            return await _context.ImportData
                .FirstOrDefaultAsync(x => x.FileName == fileName && !x.IsDeleted);
        }

        public async Task<long> AddAsync(ImportData importData)
        {
            _context.ImportData.Add(importData);
            await _context.SaveChangesAsync();
            return importData.Id;
        }

        public async Task<bool> AddDetailAsync(ImportDataDetail detail)
        {
            _context.ImportDataDetail.Add(detail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddDetailRangeAsync(IEnumerable<ImportDataDetail> details)
        {
            await _context.ImportDataDetail.AddRangeAsync(details);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStatisticsAsync(long importDataId, int successCount, int failureCount)
        {
            var importData = await _context.ImportData.FindAsync(importDataId);
            if (importData == null) return false;

            importData.SuccessfullRecords = successCount;
            importData.FailureRecords = failureCount;
            importData.UpdatedOn = System.DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
