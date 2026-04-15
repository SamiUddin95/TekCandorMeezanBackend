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
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<ChequeInfo>> GetAllAsync()
        {
            return await _context.ChequeInfo
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


        public async Task<List<ChequeInfo>> GetByBranchIdAndDateAsync(string receiverBranchCode, DateTime date)
        {
            var dateOnly = date.Date;
            return await _context.ChequeInfo
                .Where(c => c.ReceiverBranchCode == receiverBranchCode && c.Date.HasValue && c.Date.Value.Date == dateOnly)
                .OrderBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<List<ChequeInfo>> GetByStatusAsync(string status)
        {
            return await _context.ChequeInfo
                .Where(c => c.Status == status)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(long id, string status, string userId)
        {
            var cheque = await GetByIdAsync(id);
            if (cheque == null) return false;

            cheque.Status = status;
            cheque.UpdatedBy = userId;
            cheque.UpdatedOn = DateTime.Now;

            _context.ChequeInfo.Update(cheque);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
