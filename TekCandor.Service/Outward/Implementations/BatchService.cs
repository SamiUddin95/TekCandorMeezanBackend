using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IChequeInfoRepository _chequeInfoRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly AppDbContext _context;

        public BatchService(IBatchRepository batchRepository, IChequeInfoRepository chequeInfoRepository, IBranchRepository branchRepository, AppDbContext context)
        {
            _batchRepository = batchRepository;
            _chequeInfoRepository = chequeInfoRepository;
            _branchRepository = branchRepository;
            _context = context;
        }

        public async Task<BatchDTO> CreateBatchAsync(CreateBatchDTO dto, string userId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var existingBatch = await _batchRepository
                .GetByBranchAndDateAsync(dto.Branch, today, tomorrow);

            if (existingBatch != null)
            {
                throw new Exception("This branch already has a batch for today.");
            }

            var batchId = GenerateBatchId();

            var batch = new Batch
            {
                BatchId = batchId,
                Branch = dto.Branch,
                TotalInstruments = 0,
                TotalAmount = 0,
                Status = "Draft",
                MaxInstruments = dto.MaxInstruments ?? 50,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userId
            };

            var created = await _batchRepository.CreateAsync(batch);
            return MapToDTO(created);
        }

        public async Task<BatchDTO?> GetBatchByIdAsync(long id)
        {
            var batch = await _batchRepository.GetByIdAsync(id);
            return batch != null ? MapToDTO(batch) : null;
        }

        public async Task<BatchDTO?> GetBatchByBatchIdAsync(string batchId)
        {
            var batch = await _batchRepository.GetByBatchIdAsync(batchId);
            return batch != null ? MapToDTO(batch) : null;
        }

        public async Task<List<BatchDTO>> GetAllBatchesAsync()
        {
            var batches = await _batchRepository.GetAllAsync();
            return batches.Select(b => MapToDTO(b)).ToList();
        }

        public async Task<List<BatchDTO>> GetBatchesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var batches = await _batchRepository.GetByDateRangeAsync(fromDate, toDate);

            var branchesQueryable = await _branchRepository.GetAllQueryableAsync();
            var branches = await branchesQueryable.ToListAsync();
            var branchDict = branches
                .Where(b => !string.IsNullOrEmpty(b.Code))
                .GroupBy(b => b.Code)
                .ToDictionary(g => g.Key!, g => g.First().Name ?? "");

            return batches.Select(batch => MapToDTO(batch, branchDict)).ToList();
        }

        public async Task<BatchDTO?> UpdateBatchAsync(long id, CreateBatchDTO dto, string userId)
        {
            var existing = await _batchRepository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Branch = dto.Branch;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = userId;

            var updated = await _batchRepository.UpdateAsync(existing);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<bool> DeleteBatchAsync(long id)
        {
            return await _batchRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateBatchTotalsAsync(string batchId)
        {
            return await _batchRepository.UpdateBatchTotalsAsync(batchId);
        }

        public async Task<BatchDTO?> SaveBatchAsDraftAsync(string batchId, string userId)
        {
            var batch = await _batchRepository.GetByBatchIdAsync(batchId);
            if (batch == null) return null;

            if (batch.Status == "Authorized")
                throw new InvalidOperationException("Authorized batches cannot be modified");

            if (batch.Status == "Rejected")
                throw new InvalidOperationException("Rejected batches cannot be modified");

            await UpdateBatchTotalsAsync(batchId);
            batch = await _batchRepository.GetByBatchIdAsync(batchId);

            batch.Status = "Balanced";
            batch.UpdatedAt = DateTime.Now;
            batch.UpdatedBy = userId;

            var updated = await _batchRepository.UpdateAsync(batch);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<BatchDTO?> SubmitBatchForAuthorizationAsync(string batchId, string userId)
        {
            var batch = await _batchRepository.GetByBatchIdAsync(batchId);
            if (batch == null) return null;

            if (batch.Status != "Balanced")
                throw new InvalidOperationException("Only Balanced batches can be submitted for authorization");

            await UpdateBatchTotalsAsync(batchId);
            batch = await _batchRepository.GetByBatchIdAsync(batchId);
            // ? Update cheque status
            var cheques = await _context.ChequeInfo
                .Where(c => c.BatchId == batchId)
                .ToListAsync();

            foreach (var cheque in cheques)
            {
                cheque.Status = "U";
            }

            batch.Status = "Authorize";
            batch.SubmittedAt = DateTime.Now;
            batch.SubmittedBy = userId;
            batch.UpdatedAt = DateTime.Now;
            batch.UpdatedBy = userId;

            var updated = await _batchRepository.UpdateAsync(batch);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<BatchDTO?> AuthorizeBatchAsync(string batchId, string userId)
        {
            var batch = await _batchRepository.GetByBatchIdAsync(batchId);
            if (batch == null) return null;

            if (batch.Status != "Balanced")
                throw new InvalidOperationException("Only balanced batches can be authorized");

            batch.Status = "Authorized";
            batch.AuthorizedAt = DateTime.Now;
            batch.AuthorizedBy = userId;
            batch.UpdatedAt = DateTime.Now;
            batch.UpdatedBy = userId;

            var updated = await _batchRepository.UpdateAsync(batch);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<BatchDTO?> RejectBatchAsync(string batchId, string userId, string rejectionReason)
        {
            var batch = await _batchRepository.GetByBatchIdAsync(batchId);
            if (batch == null) return null;

            if (batch.Status != "Balanced")
                throw new InvalidOperationException("Only balanced batches can be rejected");

            batch.Status = "Rejected";
            batch.RejectedAt = DateTime.Now;
            batch.RejectedBy = userId;
            batch.RejectionReason = rejectionReason;
            batch.UpdatedAt = DateTime.Now;
            batch.UpdatedBy = userId;

            var updated = await _batchRepository.UpdateAsync(batch);
            return updated != null ? MapToDTO(updated) : null;
        }

        public async Task<BatchStatisticsDTO> GetBatchStatisticsAsync(DateTime fromDate, DateTime toDate)
        {
            return new BatchStatisticsDTO
            {
                TotalBatchesToday = await _batchRepository.GetTotalBatchesTodayAsync(fromDate, toDate),
                PendingAuthorization = await _batchRepository.GetPendingAuthorizationCountAsync(fromDate, toDate),
                AuthorizedValue = await _batchRepository.GetAuthorizedValueAsync(fromDate, toDate),
                ProcessingExceptions = await _batchRepository.GetProcessingExceptionsCountAsync(fromDate, toDate)
            };
        }

        public async Task<BatchDateRangeWithStatsDTO> GetBatchesByDateRangeWithStatsAsync(DateTime fromDate, DateTime toDate)
        {
            var batches = await GetBatchesByDateRangeAsync(fromDate, toDate);
            var statistics = await GetBatchStatisticsAsync(fromDate, toDate);

            return new BatchDateRangeWithStatsDTO
            {
                Batches = batches,
                Statistics = statistics
            };
        }

        public async Task<BatchWithInstrumentsDTO?> GetBatchWithInstrumentsAsync(string batchId)
        {
            var batch = await _batchRepository.GetByBatchIdAsync(batchId);
            if (batch == null) return null;

            var instruments = await GetInstrumentsByBatchIdAsync(batchId);

            return new BatchWithInstrumentsDTO
            {
                Batch = MapToDTO(batch),
                Instruments = instruments
            };
        }

        public async Task<List<ChequeInfoDTO>> GetInstrumentsByBatchIdAsync(string batchId)
        {
            var cheques = await _chequeInfoRepository.GetByBatchIdAsync(batchId);

            var banks = await _context.Bank.ToListAsync();
            var bankDict = banks
                .Where(b => !string.IsNullOrEmpty(b.Code))
                .GroupBy(b => b.Code)
                .ToDictionary(g => g.Key!, g => g.First().Name ?? "");

            return cheques.Select(c => MapChequeToDTO(c, bankDict)).ToList();
        }

        private string GenerateBatchId()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var random = new Random().Next(100, 999);
            return $"BATCH-{timestamp}-{random}";
        }

        private BatchDTO MapToDTO(Batch batch, Dictionary<string, string>? branchDict = null)
        {
            string? branchName = null;
            if (branchDict != null && !string.IsNullOrEmpty(batch.Branch))
            {
                branchDict.TryGetValue(batch.Branch, out branchName);
            }

            return new BatchDTO
            {
                Id = batch.Id,
                BatchId = batch.BatchId,
                Branch = batch.Branch,
                BranchName = branchName,
                TotalInstruments = batch.TotalInstruments,
                TotalAmount = batch.TotalAmount,
                Status = batch.Status,
                MaxInstruments = batch.MaxInstruments,
                CreatedAt = batch.CreatedAt,
                CreatedBy = batch.CreatedBy,
                UpdatedAt = batch.UpdatedAt,
                UpdatedBy = batch.UpdatedBy,
                SubmittedAt = batch.SubmittedAt,
                SubmittedBy = batch.SubmittedBy,
                AuthorizedAt = batch.AuthorizedAt,
                AuthorizedBy = batch.AuthorizedBy,
                RejectedAt = batch.RejectedAt,
                RejectedBy = batch.RejectedBy,
                RejectionReason = batch.RejectionReason
            };
        }

        private ChequeInfoDTO MapChequeToDTO(ChequeInfo entity, Dictionary<string, string>? bankDict = null)
        {
            string? drawerBankName = null;
            if (bankDict != null && !string.IsNullOrEmpty(entity.DrawerBank))
            {
                bankDict.TryGetValue(entity.DrawerBank, out drawerBankName);
            }

            return new ChequeInfoDTO
            {
                Id = entity.Id,
                Date = entity.Date,
                DepositorType = entity.DepositorType,
                AccountNo = entity.AccountNo,
                CNIC = entity.CNIC,
                DepositorTitle = entity.DepositorTitle,
                BeneficiaryAccountNumber = entity.BeneficiaryAccountNumber,
                BeneficiaryTitle = entity.BeneficiaryTitle,
                AccountStatus = entity.AccountStatus,
                BeneficiaryBranchCode = entity.BeneficiaryBranchCode,
                ChequeNo = entity.ChequeNo,
                PayingBankCode = entity.PayingBankCode,
                PayingBranchCode = entity.PayingBranchCode,
                Amount = entity.Amount,
                ChequeDate = entity.ChequeDate,
                InstrumentType = entity.InstrumentType,
                MICR = entity.MICR,
                OCREngine = entity.OCREngine,
                ProcessingTime = entity.ProcessingTime,
                Accuracy = entity.Accuracy,
                ImageF = entity.ImageF,
                ImageB = entity.ImageB,
                ImageU = entity.ImageU,
                Currency = entity.Currency,
                Remarks = entity.Remarks,
                ReceiverBranchCode = entity.ReceiverBranchCode,
                BranchName = entity.BranchName,
                DrawerBank = entity.DrawerBank,
                DrawerBankName = drawerBankName,
                AmountInWords = entity.AmountInWords,
                ReferenceNo = entity.ReferenceNo,
                DepositSlipId = entity.DepositSlipId,
                Status = entity.Status,
                IsReconciled = entity.IsReconciled,
                IsReturned = entity.IsReturned,
                IsRealized = entity.IsRealized,
                CreatedOn = entity.CreatedOn,
                CreatedBy = entity.CreatedBy,
                UpdatedOn = entity.UpdatedOn,
                UpdatedBy = entity.UpdatedBy,
                Hubcode = entity.Hubcode,
                //BatchId = entity.BatchId
            };
        }
    }
}
