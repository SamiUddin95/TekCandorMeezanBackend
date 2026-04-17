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

        public async Task<ChequeInfo?> FindByChequeDetailsAsync(string chequeNo, decimal amount, string micr)
        {
            return await _context.ChequeInfo
                .FirstOrDefaultAsync(c => c.ChequeNo == chequeNo && c.Amount == amount && c.MICR == micr);
        }

        public async Task<bool> UpdateMatchStatusAndStatusAsync(long id, string matchStatus, string status)
        {
            var cheque = await GetByIdAsync(id);
            if (cheque == null) return false;

            cheque.MatchStatus = matchStatus;
            cheque.Status = status;
            cheque.UpdatedOn = DateTime.Now;

            _context.ChequeInfo.Update(cheque);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRejectStatusAsync(long id, string status, string userId, string remarks)
        {
            var cheque = await GetByIdAsync(id);
            if (cheque == null) return false;

            cheque.Remarks = remarks;
            cheque.Status = status;
            cheque.UpdatedBy = userId;
            cheque.UpdatedOn = DateTime.Now;

            _context.ChequeInfo.Update(cheque);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetReturnListAsync()
        {
            var query = from c in _context.ChequeInfo
                        join n in _context.NiftUploadStaging on c.ChequeNo equals n.ChequeNo
                        where n.Status == "RETURN"
                        select new
                        {
                            ChequeInfoId = c.Id,
                            Date = c.Date,
                            DepositorType = c.DepositorType,
                            AccountNo = c.AccountNo,
                            CNIC = c.CNIC,
                            DepositorTitle = c.DepositorTitle,
                            BranchName = c.BranchName,
                            ChequeNo = c.ChequeNo,
                            Amount = c.Amount,
                            MICR = c.MICR,
                            Status = c.Status,
                            MatchStatus = c.MatchStatus,
                            NiftStagingId = n.Id,
                            FileName = n.FileName,
                            UploadDate = n.UploadDate,
                            ReturnCode = n.ReturnCode,
                            ReturnReason = n.ReturnReason,
                            IsProcessed = n.IsProcessed
                        };

            var result = await query.ToListAsync();
            return result.Cast<object>().ToList();
        }

        public async Task<object?> GetReturnDetailByIdAsync(long id)
        {
            var query = from c in _context.ChequeInfo
                        join n in _context.NiftUploadStaging on c.ChequeNo equals n.ChequeNo
                        where c.Id == id
                        select new
                        {
                            BeneficiaryTitle = c.BeneficiaryTitle,
                            AccountNo = c.AccountNo,
                            ChequeDate = c.Date,
                            BranchName = c.BranchName,
                            ReturnReason = n.ReturnReason,
                            ChequeNo = n.ChequeNo,
                            Amount = c.Amount,
                            ImageF = c.ImageF,
                            ImageB = c.ImageB,
                            ImageU = c.ImageU
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<object>> GetFundRealizationListAsync()
        {
            var query = from c in _context.ChequeInfo
                        join n in _context.NiftUploadStaging on c.ChequeNo equals n.ChequeNo
                        where n.Status == "PAID"
                        group c by new { c.ReceiverBranchCode, c.BranchName } into g
                        select new
                        {
                            ReceiverBranchCode = g.Key.ReceiverBranchCode,
                            BranchName = g.Key.BranchName,
                            TotalAmount = g.Sum(x => x.Amount ?? 0),
                            ChequeCount = g.Count()
                        };

            var result = await query.ToListAsync();
            return result.Cast<object>().ToList();
        }

        public async Task<bool> MarkAsReturnAsync(long id, string userId)
        {
            var cheque = await GetByIdAsync(id);
            if (cheque == null) return false;

            cheque.Status = "R";
            cheque.UpdatedBy = userId;
            cheque.UpdatedOn = DateTime.Now;

            _context.ChequeInfo.Update(cheque);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
