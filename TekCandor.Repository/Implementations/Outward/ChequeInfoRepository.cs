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
    public class ChequeInfoRepository : IChequeInfoRepository
    {
        private readonly AppDbContext _context;

        public ChequeInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ChequeInfo> CreateAsync(ChequeInfo chequeInfo)
        {
            chequeInfo.CreatedOn = DateTime.Now;
            _context.ChequeInfo.Add(chequeInfo);
            await _context.SaveChangesAsync();
            return chequeInfo;
        }

        public async Task<ChequeInfo?> GetByIdAsync(long id)
        {
            return await _context.ChequeInfo
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<ChequeInfo>> GetAllAsync()
        {
            return await _context.ChequeInfo
                .Include(c => c.Branch)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<ChequeInfo> UpdateAsync(ChequeInfo chequeInfo)
        {
            chequeInfo.UpdatedOn = DateTime.Now;
            _context.ChequeInfo.Update(chequeInfo);
            await _context.SaveChangesAsync();
            return chequeInfo;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var chequeInfo = await GetByIdAsync(id);
            if (chequeInfo == null) return false;

            _context.ChequeInfo.Remove(chequeInfo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChequeInfo>> GetByBranchIdAsync(long branchId)
        {
            return await _context.ChequeInfo
                .Include(c => c.Branch)
                .Where(c => c.BranchId == branchId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<ChequeInfo>> GetByStatusAsync(string status)
        {
            return await _context.ChequeInfo
                .Include(c => c.Branch)
                .Where(c => c.Status == status)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }
    }
}
