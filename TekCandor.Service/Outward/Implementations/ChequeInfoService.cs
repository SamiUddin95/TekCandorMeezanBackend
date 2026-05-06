using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Implementations;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class ChequeInfoService : IChequeInfoService
    {
        private readonly IChequeInfoRepository _repository;
        private readonly INiftUploadStagingRepository _niftRepository;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private static readonly Random _random = new Random();
        public ChequeInfoService(IChequeInfoRepository repository, INiftUploadStagingRepository niftRepository, AppDbContext context, IUserRepository userRepository)
        {
            _repository = repository;
            _niftRepository = niftRepository;
            _context = context;
            _userRepository = userRepository;
        }

        public int Generate7DigitNumber()
        {
            Random random = new Random();
            return random.Next(1000000, 10000000); // 7-digit range
        }

        public decimal GenerateRandomAmount(decimal min, decimal max)
        {
            Random random = new Random();
            double value = random.NextDouble() * (double)(max - min) + (double)min;
            return Math.Round((decimal)value, 2);
        }

        public string GenerateFormattedNumber()
        {
            int part1 = _random.Next(100000000, 1000000000); // 9 digits
            int part2 = _random.Next(10000000, 100000000);   // 8 digits
            int part3 = _random.Next(0, 100);                // 2 digits (00–99)

            return $"{part1}: {part2:D8}: {part3:D2}";
        }
        public async Task<ChequeInfoDTO> CreateAsync(ChequeInfoDTO dto, string userId)
        {
            var getUser = await _context.Users
            .Where(x => x.LoginName == userId)
            .FirstOrDefaultAsync();

            var getHubCode = string.Empty;

            var getHubId = await _context.Branch.Where(x => x.Code == dto.ReceiverBranchCode).Select(b => b.HubId).FirstOrDefaultAsync();
            if (getHubId != null)
            {
                getHubCode = await _context.Hub.Where(x => x.Id == getHubId).Select(h => h.Code).FirstOrDefaultAsync();
            }
            else
            {
                getHubCode = "10";
            }
            long userIdLong = getUser.Id;
            var upperLimit = await _userRepository.GetUserUpperLimitAsync(userIdLong);
            var entity = new ChequeInfo
            {
                Date = dto.Date,
                DepositorType = dto.DepositorType,
                AccountNo = dto.AccountNo,
                CNIC = dto.CNIC,
                DepositorTitle = dto.DepositorTitle,
                BeneficiaryAccountNumber = dto.BeneficiaryAccountNumber,
                BeneficiaryTitle = dto.BeneficiaryTitle,
                AccountStatus = dto.AccountStatus,
                BeneficiaryBranchCode = dto.BeneficiaryBranchCode,
                ChequeNo = Convert.ToString(Generate7DigitNumber()),//dto.ChequeNo,
                PayingBankCode = dto.PayingBankCode,
                PayingBranchCode = dto.PayingBranchCode,
                Amount = dto.Amount,//GenerateRandomAmount(1000, 100000),//dto.Amount,
                ChequeDate = dto.ChequeDate,
                InstrumentType = dto.InstrumentType,
                MICR = GenerateFormattedNumber(),//dto.MICR,
                OCREngine = dto.OCREngine,
                ProcessingTime = dto.ProcessingTime,
                Accuracy = dto.Accuracy,
                ImageF = dto.ImageF,
                ImageB = dto.ImageB,
                ImageU = dto.ImageU,
                Currency = dto.Currency,
                Remarks = dto.Remarks,
                ReceiverBranchCode = dto.ReceiverBranchCode,
                DrawerBank = dto.DrawerBank,
                AmountInWords = dto.AmountInWords,
                ReferenceNo = dto.ReferenceNo,
                DepositSlipId = dto.DepositSlipId,
                Status = (upperLimit.HasValue && dto.Amount > upperLimit.Value) ? "U" : "A",
                //Status = dto.Status ?? "Pending",
                Hubcode = getHubCode,
                //BatchId = dto.BatchId,
                IsReconciled = dto.IsReconciled,
                IsReturned = dto.IsReturned,
                IsRealized = dto.IsRealized,
                CreatedBy = userId
            };

            var created = await _repository.CreateAsync(entity);
            return MapToDTO(created);
        }

        public async Task<ChequeInfoDTO?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }


        public async Task<PagedResult<ChequeInfoDTO>> GetAllPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var (items, totalCount) = await _repository.GetAllPagedAsync(pageNumber, pageSize, fromDate, toDate);

            var dtos = items.Select(e => MapToDTO(e)).ToList();

            return new PagedResult<ChequeInfoDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<ChequeInfoDTO?> UpdateAsync(long id, ChequeInfoDTO dto, string userId)
        {
            var getUser = await _context.Users
            .Where(x => x.LoginName == userId)
            .FirstOrDefaultAsync();

            var getHubCode = string.Empty;

            var getHubId = await _context.Branch.Where(x => x.Code == dto.ReceiverBranchCode).Select(b => b.HubId).FirstOrDefaultAsync();
            if (getHubId != null)
            {
                getHubCode = await _context.Hub.Where(x => x.Id == getHubId).Select(h => h.Code).FirstOrDefaultAsync();
            }
            else
            {
                getHubCode = "10";
            }
            long userIdLong = getUser.Id;
            var upperLimit = await _userRepository.GetUserUpperLimitAsync(userIdLong);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Date = dto.Date;
            entity.DepositorType = dto.DepositorType;
            entity.AccountNo = dto.AccountNo;
            entity.CNIC = dto.CNIC;
            entity.DepositorTitle = dto.DepositorTitle;
            entity.BeneficiaryAccountNumber = dto.BeneficiaryAccountNumber;
            entity.BeneficiaryTitle = dto.BeneficiaryTitle;
            entity.AccountStatus = dto.AccountStatus;
            entity.BeneficiaryBranchCode = dto.BeneficiaryBranchCode;
            entity.ChequeNo = dto.ChequeNo;
            entity.PayingBankCode = dto.PayingBankCode;
            entity.PayingBranchCode = dto.PayingBranchCode;
            entity.Amount = dto.Amount;
            entity.ChequeDate = dto.ChequeDate;
            entity.InstrumentType = dto.InstrumentType;
            entity.MICR = dto.MICR;
            entity.OCREngine = dto.OCREngine;
            entity.ProcessingTime = dto.ProcessingTime;
            entity.Accuracy = dto.Accuracy;
            entity.ImageF = dto.ImageF;
            entity.ImageB = dto.ImageB;
            entity.ImageU = dto.ImageU;
            entity.Currency = dto.Currency;
            entity.Remarks = dto.Remarks;
            entity.ReceiverBranchCode = dto.ReceiverBranchCode;
            entity.DrawerBank = dto.DrawerBank;
            entity.AmountInWords = dto.AmountInWords;
            entity.ReferenceNo = dto.ReferenceNo;
            entity.DepositSlipId = dto.DepositSlipId;
            //entity.Status = dto.Status;
            entity.Status = (upperLimit.HasValue && dto.Amount > upperLimit.Value) ? "U" : "A";
            //Status = dto.Status ?? "Pending",
            entity.Hubcode = getHubCode;
            //entity.BatchId = dto.BatchId;
            entity.IsReconciled = dto.IsReconciled;
            entity.IsReturned = dto.IsReturned;
            entity.IsRealized = dto.IsRealized;
            entity.UpdatedBy = userId;

            var updated = await _repository.UpdateAsync(entity);
            return MapToDTO(updated);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<ChequeInfoDTO>> GetByBranchIdAndDateAsync(string receiverBranchCode, DateTime date)
        {
            var entities = await _repository.GetByBranchIdAndDateAsync(receiverBranchCode, date);
            return entities.Select(e => MapToDTO(e)).ToList();
        }

        public async Task<List<ChequeInfoDTO>> GetByHubcodeAndDateAsync(string hubcode, DateTime date)
        {
            var entities = await _repository.GetByHubcodeAndDateAsync(hubcode, date);
            return entities.Select(e => MapToDTO(e)).ToList();
        }

        public async Task<List<ChequeInfoDTO>> GetByStatusAsync(string status, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var entities = await _repository.GetByStatusAsync(status, fromDate, toDate);
            return entities.Select(e => MapToDTO(e)).ToList();
        }

        public async Task<bool> ApproveAsync(long id, string userId)
        {
            return await _repository.UpdateStatusAsync(id, "Approved", userId);
        }

        public async Task<bool> RejectAsync(long id, string userId, string remarks)
        {
            return await _repository.UpdateRejectStatusAsync(id, "Rejected", userId, remarks);
        }

        public async Task<string> GenerateFileContentAsync(string receiverBranchCode, DateTime date)
        {
            var cheques = await _repository.GetByBranchIdAndDateAsync(receiverBranchCode, date);

            if (!cheques.Any())
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var cheque in cheques)
            {
                sb.AppendLine($"{cheque.ChequeNo}|{cheque.Amount}|{cheque.MICR}|{cheque.ReceiverBranchCode}|{cheque.Date:yyyy-MM-dd}");
            }

            return sb.ToString();
        }

        public async Task<NiftUploadResultDTO> ProcessNiftFileAsync(string fileName, string fileContent, string fileType)
        {
            return new NiftUploadResultDTO
            {
                MatchedRecords = new List<NiftRecordDTO>(),
                UnmatchedRecords = new List<NiftRecordDTO>(),
                Summary = new NiftSummaryDTO
                {
                    TotalLodgement = 0,
                    Matched = 0,
                    Unmatched = 0,
                    TotalAmount = 0
                }
            };
        }

        public async Task<NiftUploadResultDTO> GetNiftUploadDataAsync(DateTime date)
        {
            return new NiftUploadResultDTO
            {
                MatchedRecords = new List<NiftRecordDTO>(),
                UnmatchedRecords = new List<NiftRecordDTO>(),
                Summary = new NiftSummaryDTO
                {
                    TotalLodgement = 0,
                    Matched = 0,
                    Unmatched = 0,
                    TotalAmount = 0
                }
            };
        }

        public async Task<bool> ForceMatchAsync(ForceMatchRequestDTO request)
        {
            var niftRecord = await _niftRepository.GetByIdAsync(request.NiftStagingId);
            if (niftRecord == null)
                return false;

            var cheque = await _repository.FindByChequeDetailsAsync(request.ChequeNo, niftRecord.Amount ?? 0, niftRecord.MICR ?? "");
            if (cheque == null)
                return false;

            cheque.Status = niftRecord.Status;
            cheque.UpdatedOn = DateTime.Now;

            await _repository.UpdateMatchStatusAndStatusAsync(cheque.Id, "Matched", "Matched");

            niftRecord.IsProcessed = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<ReturnListDTO>> GetReturnListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var (items, totalCount) = await _repository.GetReturnListPagedAsync(pageNumber, pageSize, fromDate, toDate);

            var dtos = items.Select(item => new ReturnListDTO
            {
                ChequeInfoId = (long)((dynamic)item).ChequeInfoId,
                Date = ((dynamic)item).Date != null ? (DateTime?)((dynamic)item).Date : null,
                DepositorType = ((dynamic)item).DepositorType?.ToString(),
                AccountNo = ((dynamic)item).AccountNo?.ToString(),
                CNIC = ((dynamic)item).CNIC?.ToString(),
                DepositorTitle = ((dynamic)item).DepositorTitle?.ToString(),
                BranchName = ((dynamic)item).BranchName?.ToString(),
                ChequeNo = ((dynamic)item).ChequeNo?.ToString() ?? "",
                Amount = ((dynamic)item).Amount != null ? (decimal?)((dynamic)item).Amount : null,
                MICR = ((dynamic)item).MICR?.ToString(),
                Status = ((dynamic)item).Status?.ToString() ?? "",
                MatchStatus = ((dynamic)item).MatchStatus?.ToString(),
                NiftStagingId = ((dynamic)item).NiftStagingId != null ? (long)((dynamic)item).NiftStagingId : 0,
                FileName = ((dynamic)item).FileName?.ToString(),
                UploadDate = ((dynamic)item).UploadDate != null ? (DateTime?)((dynamic)item).UploadDate : null,
                ReturnCode = ((dynamic)item).ReturnCode?.ToString(),
                ReturnReason = ((dynamic)item).ReturnReason?.ToString(),
                IsProcessed = ((dynamic)item).IsProcessed != null ? (bool?)((dynamic)item).IsProcessed : null
            }).ToList();

            return new PagedResult<ReturnListDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ReturnDetailDTO?> GetReturnDetailByIdAsync(long id)
        {
            var item = await _repository.GetReturnDetailByIdAsync(id);
            if (item == null) return null;

            return new ReturnDetailDTO
            {
                BeneficiaryTitle = ((dynamic)item).BeneficiaryTitle?.ToString(),
                AccountNo = ((dynamic)item).AccountNo?.ToString(),
                ChequeDate = ((dynamic)item).ChequeDate != null ? (DateTime?)((dynamic)item).ChequeDate : null,
                BranchName = ((dynamic)item).BranchName?.ToString(),
                ReturnReason = ((dynamic)item).ReturnReason?.ToString(),
                ChequeNo = ((dynamic)item).ChequeNo?.ToString() ?? "",
                Amount = ((dynamic)item).Amount != null ? (decimal?)((dynamic)item).Amount : null,
                ImageF = ((dynamic)item).ImageF?.ToString(),
                ImageB = ((dynamic)item).ImageB?.ToString(),
                ImageU = ((dynamic)item).ImageU?.ToString()
            };
        }

        public async Task<PagedResult<FundRealizationDTO>> GetFundRealizationListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var (items, totalCount) = await _repository.GetFundRealizationListPagedAsync(pageNumber, pageSize, fromDate, toDate);

            var dtos = items.Select(item => new FundRealizationDTO
            {
                ReceiverBranchCode = ((dynamic)item).ReceiverBranchCode?.ToString(),
                BranchName = ((dynamic)item).BranchName?.ToString(),
                TotalAmount = ((dynamic)item).TotalAmount != null ? (decimal)((dynamic)item).TotalAmount : 0,
                ChequeCount = ((dynamic)item).ChequeCount != null ? (int)((dynamic)item).ChequeCount : 0
            }).ToList();

            return new PagedResult<FundRealizationDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> MarkAsReturnAsync(long id, string userId)
        {
            return await _repository.MarkAsReturnAsync(id, userId);
        }

        private ChequeInfoDTO MapToDTO(ChequeInfo entity, Dictionary<string, string>? bankDict = null)
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

        public async Task<BulkApproveResponseDTO> BulkSupervisorApproveAsync(BulkApproveRequestDTO request, string userId)
        {
            var response = new BulkApproveResponseDTO
            {
                TotalRequested = request.ChequeIds.Count
            };

            if (request.ChequeIds.Count == 0)
            {
                return response;
            }

            try
            {
                var updatedCount = await _repository.BulkUpdateStatusAsync(request.ChequeIds, "A", userId);

                response.SuccessCount = updatedCount;
                response.FailedCount = request.ChequeIds.Count - updatedCount;

                if (response.FailedCount > 0)
                {
                    var existingCheques = await _repository.GetByStatusAsync("A");
                    var approvedIds = existingCheques
                        .Where(c => request.ChequeIds.Contains(c.Id))
                        .Select(c => c.Id)
                        .ToList();

                    response.FailedIds = request.ChequeIds
                        .Where(id => !approvedIds.Contains(id))
                        .ToList();
                }
            }
            catch (Exception)
            {
                response.FailedCount = request.ChequeIds.Count;
                response.FailedIds = request.ChequeIds;
            }

            return response;
        }

        //public async Task<PagedResult<ChequeInfoDTO>> GetSupervisorListPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        //{
        //    var (items, totalCount) = await _repository.GetByStatusPagedAsync("U", pageNumber, pageSize, fromDate, toDate);

        //    var dtos = items.Select(e => MapToDTO(e)).ToList();

        //    return new PagedResult<ChequeInfoDTO>
        //    {
        //        Items = dtos,
        //        TotalCount = totalCount,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    };
        //}

        public async Task<SupervisorListGroupedResult> GetSupervisorListGroupedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            
            var allCheques = await _repository.GetByStatusAsync("U", fromDate, toDate);

           
            var batchIds = allCheques.Select(c => c.BatchId).Distinct().Where(b => !string.IsNullOrEmpty(b)).ToList();

           
            var batches = await _context.Batch
                .Where(b => batchIds.Contains(b.BatchId))
                .ToListAsync();

           
            var branchCodes = batches.Select(b => b.Branch).Distinct().Where(b => !string.IsNullOrEmpty(b)).ToList();
            var branches = await _context.Branch
                .Where(b => branchCodes.Contains(b.Code))
                .ToListAsync();
            var branchDict = branches
                .Where(b => !string.IsNullOrEmpty(b.Code))
                .GroupBy(b => b.Code)
                .ToDictionary(g => g.Key!, g => g.First().Name ?? "");

          
            var bankCodes = allCheques.Select(c => c.DrawerBank).Distinct().Where(b => !string.IsNullOrEmpty(b)).ToList();
            var banks = await _context.Bank
                .Where(b => bankCodes.Contains(b.Code))
                .ToListAsync();
            var bankDict = banks
                .Where(b => !string.IsNullOrEmpty(b.Code))
                .GroupBy(b => b.Code)
                .ToDictionary(g => g.Key!, g => g.First().Name ?? "");

            
            var groupedBatches = allCheques
                .GroupBy(c => c.BatchId)
                .Select(g =>
                {
                    var batch = batches.FirstOrDefault(b => b.BatchId == g.Key);
                    var branchName = batch != null && !string.IsNullOrEmpty(batch.Branch) && branchDict.ContainsKey(batch.Branch)
                        ? branchDict[batch.Branch]
                        : null;

                    return new BatchGroupedChequeDTO
                    {
                        BatchId = g.Key ?? "",
                    
                        BranchName = branchName,
                       
                        Items = g.Select(c => MapToDTO(c, bankDict)).ToList()
                    };
                })
                .OrderByDescending(b => b.BatchId)
                .ToList();

            
            var totalCount = groupedBatches.Count;
            var pagedBatches = groupedBatches
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new SupervisorListGroupedResult
            {
                Batches = pagedBatches,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<ChequeInfoDTO>> GetTransactionHistoryPagedAsync(int pageNumber, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var (items, totalCount) = await _repository.GetTransactionHistoryPagedAsync(pageNumber, pageSize, fromDate, toDate);

            var dtos = items.Select(e => MapToDTO(e)).ToList();

            return new PagedResult<ChequeInfoDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<string> GenerateFileContentHubwiseAsync(string hubcode, DateTime date)
        {
            var cheques = await _repository.GetByHubcodeAndDateAsync(hubcode, date);

            if (!cheques.Any())
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var cheque in cheques)
            {
                sb.AppendLine($"{cheque.ChequeNo}|{cheque.Amount}|{cheque.MICR}|{cheque.ReceiverBranchCode}|{cheque.Date:yyyy-MM-dd}");
            }

            return sb.ToString();
        }
    }
}
