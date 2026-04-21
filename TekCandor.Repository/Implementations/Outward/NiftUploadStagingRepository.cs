using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;

namespace TekCandor.Repository.Implementations.Outward
{
    public class NiftUploadStagingRepository : INiftUploadStagingRepository
    {
        private readonly AppDbContext _context;

        public NiftUploadStagingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NiftUploadStaging> CreateAsync(NiftUploadStaging niftUpload)
        {
            _context.NiftUploadStaging.Add(niftUpload);
            await _context.SaveChangesAsync();
            return niftUpload;
        }

        public async Task<List<NiftUploadStaging>> CreateBulkAsync(List<NiftUploadStaging> niftUploads)
        {
            _context.NiftUploadStaging.AddRange(niftUploads);
            await _context.SaveChangesAsync();
            return niftUploads;
        }

        public async Task<bool> UpdateIsProcessedAsync(long id, bool isProcessed)
        {
            var record = await _context.NiftUploadStaging.FindAsync(id);
            if (record == null) return false;

            record.IsProcessed = isProcessed;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<NiftUploadStaging>> GetByUploadDateAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _context.NiftUploadStaging
                .Where(n => n.UploadDate.HasValue && n.UploadDate.Value.Date == dateOnly)
                .OrderByDescending(n => n.Id)
                .ToListAsync();
        }

        public async Task<NiftUploadStaging?> GetByIdAsync(long id)
        {
            return await _context.NiftUploadStaging.FindAsync(id);
        }
    }
}
