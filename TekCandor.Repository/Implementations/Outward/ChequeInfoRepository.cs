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
            try
            {
                chequeInfo.CreatedOn = DateTime.Now;
                _context.ChequeInfo.Add(chequeInfo);
                await _context.SaveChangesAsync();
                return chequeInfo;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public async Task<ChequeInfo?> GetByIdAsync(long id)
        {
            return await _context.ChequeInfo
                .FirstOrDefaultAsync(c => c.Id == id);
        }

     

        public async Task<(List<ChequeInfo> items, int totalCount)> GetAllPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var allowedStatuses = new[] { "P", "C", "RE", "A" };
            var query = _context.ChequeInfo
                .Where(c => allowedStatuses.Contains(c.Status));

            if (fromDate.HasValue)
            {
                query = query.Where(c => c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(c => c.Date <= toDate.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
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

        public async Task<List<ChequeInfo>> GetByHubcodeAndDateAsync(string hubcode, DateTime date)
        {
            var dateOnly = date.Date;
            return await _context.ChequeInfo
                .Where(c => c.Hubcode == hubcode && c.Date.HasValue && c.Date.Value.Date == dateOnly)
                .OrderBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<List<ChequeInfo>> GetByStatusAsync(string status, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.ChequeInfo
                .Where(c => c.Status == status);

            if (fromDate.HasValue)
            {
                query = query.Where(c => c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(c => c.Date <= toDate.Value);
            }

            return await query
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<(List<ChequeInfo> items, int totalCount)> GetByStatusPagedAsync(string status, int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.ChequeInfo
                .Where(c => c.Status == status);

            if (fromDate.HasValue)
            {
                query = query.Where(c => c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(c => c.Date <= toDate.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
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

       

        public async Task<(List<object> items, int totalCount)> GetReturnListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
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

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.Date <= toDate.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items.Cast<object>().ToList(), totalCount);
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



        public async Task<(List<object> items, int totalCount)> GetFundRealizationListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var baseQuery = from c in _context.ChequeInfo
                            join n in _context.NiftUploadStaging on c.ChequeNo equals n.ChequeNo
                            where n.Status == "PAID"
                            select c;

            if (fromDate.HasValue)
            {
                baseQuery = baseQuery.Where(c => c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                baseQuery = baseQuery.Where(c => c.Date <= toDate.Value);
            }

            var query = from c in baseQuery
                        group c by new { c.ReceiverBranchCode, c.BranchName } into g
                        select new
                        {
                            ReceiverBranchCode = g.Key.ReceiverBranchCode,
                            BranchName = g.Key.BranchName,
                            TotalAmount = g.Sum(x => x.Amount ?? 0),
                            ChequeCount = g.Count()
                        };

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.BranchName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items.Cast<object>().ToList(), totalCount);
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

        public async Task<int> BulkUpdateStatusAsync(List<long> ids, string status, string userId)
        {
            var cheques = await _context.ChequeInfo
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            if (cheques.Count == 0) return 0;

            foreach (var cheque in cheques)
            {
                cheque.Status = status;
                cheque.UpdatedBy = userId;
                cheque.UpdatedOn = DateTime.Now;
            }

            _context.ChequeInfo.UpdateRange(cheques);
            await _context.SaveChangesAsync();
            
            return cheques.Count;
        }

        public async Task<(List<ChequeInfo> items, int totalCount)> GetTransactionHistoryPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.ChequeInfo.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(c => c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(c => c.Date <= toDate.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<ChequeInfo>> GetByBatchIdAsync(string batchId)
        {
            return await _context.ChequeInfo
                .Where(c => c.BatchId == batchId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }
    }
}
