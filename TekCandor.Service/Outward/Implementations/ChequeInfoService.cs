using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
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
        private static readonly Random _random = new Random();
        public ChequeInfoService(IChequeInfoRepository repository, INiftUploadStagingRepository niftRepository, AppDbContext context)
        {
            _repository = repository;
            _niftRepository = niftRepository;
            _context = context;
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
                Amount = GenerateRandomAmount(1000, 100000),//dto.Amount,
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
                Status = dto.Status ?? "Pending",
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

        public async Task<List<ChequeInfoDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDTO).ToList();
        }

        public async Task<ChequeInfoDTO?> UpdateAsync(long id, ChequeInfoDTO dto, string userId)
        {
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
            entity.Status = dto.Status;
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

        public async Task<string> GenerateFileContentAsync(string receiverBranchCode, DateTime date)
        {
            var cheques = await _repository.GetByBranchIdAndDateAsync(receiverBranchCode, date);
            
            if (cheques == null || cheques.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            
            var count = cheques.Count;
            var totalAmount = cheques.Sum(c => c.Amount ?? 0);
            var dateStr = date.ToString("dd-MM-yyyy");
            
            sb.AppendLine($"{dateStr}|{count}|{totalAmount:0}");
            
            foreach (var cheque in cheques)
            {
                var chequeDate = cheque.Date.HasValue ? cheque.Date.Value.ToString("dd-MM-yyyy") : dateStr;
                var chequeNo = cheque.ChequeNo ?? "";
                var amount = cheque.Amount ?? 0;
                
                sb.AppendLine($"{chequeDate}|{chequeNo}|{amount:0}");
            }
            
            sb.AppendLine("EOF");
            
            return sb.ToString();
        }

        public async Task<List<ChequeInfoDTO>> GetByStatusAsync(string status)
        {
            var entities = await _repository.GetByStatusAsync(status);
            return entities.Select(MapToDTO).ToList();
        }

        public async Task<bool> ApproveAsync(long id, string userId)
        {
            return await _repository.UpdateStatusAsync(id, "A", userId);
        }

        public async Task<bool> RejectAsync(long id, string userId, string remarks)
        {
            return await _repository.UpdateRejectStatusAsync(id, "RE", userId, remarks);
        }

        public async Task<NiftUploadResultDTO> ProcessNiftFileAsync(string fileName, string fileContent, string fileType)
        {
            var result = new NiftUploadResultDTO();
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var stagingRecords = new List<NiftUploadStaging>();
            
            bool isPaid = fileType.ToUpper() == "PAID";
            string status = isPaid ? "PAID" : "RETURN";

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                
                if (i == 0 || line == "EOF")
                    continue;

                var parts = line.Split('|');
                
                if (isPaid && parts.Length >= 4)
                {
                    stagingRecords.Add(new NiftUploadStaging
                    {
                        FileName = fileName,
                        UploadDate = DateTime.Now,
                        ChequeNo = parts[1],
                        Amount = decimal.TryParse(parts[2], out var amt) ? amt : 0,
                        MICR = parts[3],
                        Status = status,
                        IsProcessed = false
                    });
                }
                else if (!isPaid && parts.Length >= 6)
                {
                    stagingRecords.Add(new NiftUploadStaging
                    {
                        FileName = fileName,
                        UploadDate = DateTime.Now,
                        ChequeNo = parts[1],
                        Amount = decimal.TryParse(parts[2], out var amt) ? amt : 0,
                        MICR = parts[3],
                        Status = status,
                        ReturnCode = parts[4],
                        ReturnReason = parts[5],
                        IsProcessed = false
                    });
                }
            }

            await _niftRepository.CreateBulkAsync(stagingRecords);

            var matchedList = new List<NiftRecordDTO>();
            var unmatchedList = new List<NiftRecordDTO>();
            decimal totalAmount = 0;

            foreach (var record in stagingRecords)
            {
                var chequeInfo = await _repository.FindByChequeDetailsAsync(
                    record.ChequeNo ?? "", 
                    record.Amount ?? 0, 
                    record.MICR ?? ""
                );

                var niftRecord = new NiftRecordDTO
                {
                    ChequeInfoId = chequeInfo?.Id,
                    NiftStagingId = record.Id,
                    ChequeNo = record.ChequeNo,
                    Amount = record.Amount,
                    Date = record.UploadDate?.ToString("dd-MM-yyyy"),
                    Discrepancy = isPaid ? "" : record.ReturnReason,
                    BranchName = chequeInfo?.BranchName ?? ""
                };

                if (chequeInfo != null)
                {
                    await _repository.UpdateMatchStatusAndStatusAsync(
                        chequeInfo.Id, 
                        "Matched", 
                        isPaid ? "Cleared" : "Returned"
                    );
                    
                    await _niftRepository.UpdateIsProcessedAsync(record.Id, true);
                    
                    matchedList.Add(niftRecord);
                }
                else
                {
                    unmatchedList.Add(niftRecord);
                }

                totalAmount += record.Amount ?? 0;
            }

            result.MatchedRecords = matchedList;
            result.UnmatchedRecords = unmatchedList;
            result.Summary = new NiftSummaryDTO
            {
                TotalLodgement = stagingRecords.Count,
                Matched = matchedList.Count,
                Unmatched = unmatchedList.Count,
                TotalAmount = totalAmount
            };

            return result;
        }

        public async Task<NiftUploadResultDTO> GetNiftUploadDataAsync(DateTime date)
        {
            var result = new NiftUploadResultDTO();
            var stagingRecords = await _niftRepository.GetByUploadDateAsync(date);

            if (stagingRecords == null || stagingRecords.Count == 0)
            {
                return result;
            }

            var matchedList = new List<NiftRecordDTO>();
            var unmatchedList = new List<NiftRecordDTO>();
            decimal totalAmount = 0;

            foreach (var record in stagingRecords)
            {
                var chequeInfo = await _repository.FindByChequeDetailsAsync(
                    record.ChequeNo ?? "",
                    record.Amount ?? 0,
                    record.MICR ?? ""
                );

                bool isPaid = record.Status?.ToUpper() == "PAID";

                var niftRecord = new NiftRecordDTO
                {
                    ChequeInfoId = chequeInfo?.Id,
                    NiftStagingId = record.Id,
                    ChequeNo = record.ChequeNo,
                    Amount = record.Amount,
                    Date = record.UploadDate?.ToString("dd-MM-yyyy"),
                    Discrepancy = isPaid ? "" : record.ReturnReason,
                    BranchName = chequeInfo?.BranchName ?? ""
                };

                if (record.IsProcessed == true && chequeInfo != null)
                {
                    matchedList.Add(niftRecord);
                }
                else
                {
                    unmatchedList.Add(niftRecord);
                }

                totalAmount += record.Amount ?? 0;
            }

            result.MatchedRecords = matchedList;
            result.UnmatchedRecords = unmatchedList;
            result.Summary = new NiftSummaryDTO
            {
                TotalLodgement = stagingRecords.Count,
                Matched = matchedList.Count,
                Unmatched = unmatchedList.Count,
                TotalAmount = totalAmount
            };

            return result;
        }

        public async Task<bool> ForceMatchAsync(ForceMatchRequestDTO request)
        {
            var niftRecord = await _niftRepository.GetByIdAsync(request.NiftStagingId);
            if (niftRecord == null)
                return false;

            var getchequeInfoId = await _context.ChequeInfo
                .Where(c => c.ChequeNo == request.ChequeNo)
                .Select(c => c.Id)
                .FirstOrDefaultAsync(
            );

            var chequeInfo = await _repository.GetByIdAsync(getchequeInfoId);
            if (chequeInfo == null)
                return false;

            bool isPaid = niftRecord.Status?.ToUpper() == "PAID";
            string newStatus = isPaid ? "Cleared" : "Returned";

            var updated = await _repository.UpdateMatchStatusAndStatusAsync(
                chequeInfo.Id,
                "Force Matched",
                newStatus
            );

            if (updated)
            {
                await _niftRepository.UpdateIsProcessedAsync(niftRecord.Id, true);
            }

            return updated;
        }

        public async Task<List<ReturnListDTO>> GetReturnListAsync()
        {
            var data = await _repository.GetReturnListAsync();
            
            var result = new List<ReturnListDTO>();
            foreach (var item in data)
            {
                var itemType = item.GetType();
                result.Add(new ReturnListDTO
                {
                    ChequeInfoId = (long)itemType.GetProperty("ChequeInfoId")?.GetValue(item)!,
                    Date = (DateTime?)itemType.GetProperty("Date")?.GetValue(item),
                    DepositorType = (string?)itemType.GetProperty("DepositorType")?.GetValue(item),
                    AccountNo = (string?)itemType.GetProperty("AccountNo")?.GetValue(item),
                    CNIC = (string?)itemType.GetProperty("CNIC")?.GetValue(item),
                    DepositorTitle = (string?)itemType.GetProperty("DepositorTitle")?.GetValue(item),
                    BranchName = (string?)itemType.GetProperty("BranchName")?.GetValue(item),
                    ChequeNo = (string?)itemType.GetProperty("ChequeNo")?.GetValue(item),
                    Amount = (decimal?)itemType.GetProperty("Amount")?.GetValue(item),
                    MICR = (string?)itemType.GetProperty("MICR")?.GetValue(item),
                    Status = (string?)itemType.GetProperty("Status")?.GetValue(item),
                    MatchStatus = (string?)itemType.GetProperty("MatchStatus")?.GetValue(item),
                    NiftStagingId = (long)itemType.GetProperty("NiftStagingId")?.GetValue(item)!,
                    FileName = (string?)itemType.GetProperty("FileName")?.GetValue(item),
                    UploadDate = (DateTime?)itemType.GetProperty("UploadDate")?.GetValue(item),
                    ReturnCode = (string?)itemType.GetProperty("ReturnCode")?.GetValue(item),
                    ReturnReason = (string?)itemType.GetProperty("ReturnReason")?.GetValue(item),
                    IsProcessed = (bool?)itemType.GetProperty("IsProcessed")?.GetValue(item)
                });
            }

            return result;
        }

        public async Task<ReturnDetailDTO?> GetReturnDetailByIdAsync(long id)
        {
            var data = await _repository.GetReturnDetailByIdAsync(id);
            
            if (data == null)
                return null;

            var itemType = data.GetType();
            return new ReturnDetailDTO
            {
                BeneficiaryTitle = (string?)itemType.GetProperty("BeneficiaryTitle")?.GetValue(data),
                AccountNo = (string?)itemType.GetProperty("AccountNo")?.GetValue(data),
                ChequeDate = (DateTime?)itemType.GetProperty("ChequeDate")?.GetValue(data),
                BranchName = (string?)itemType.GetProperty("BranchName")?.GetValue(data),
                ReturnReason = (string?)itemType.GetProperty("ReturnReason")?.GetValue(data),
                ChequeNo = (string?)itemType.GetProperty("ChequeNo")?.GetValue(data),
                Amount = (decimal?)itemType.GetProperty("Amount")?.GetValue(data),
                ImageF = (string?)itemType.GetProperty("ImageF")?.GetValue(data),
                ImageB = (string?)itemType.GetProperty("ImageB")?.GetValue(data),
                ImageU = (string?)itemType.GetProperty("ImageU")?.GetValue(data)
            };
        }

        public async Task<List<FundRealizationDTO>> GetFundRealizationListAsync()
        {
            var data = await _repository.GetFundRealizationListAsync();
            
            var result = new List<FundRealizationDTO>();
            foreach (var item in data)
            {
                var itemType = item.GetType();
                result.Add(new FundRealizationDTO
                {
                    ReceiverBranchCode = (string?)itemType.GetProperty("ReceiverBranchCode")?.GetValue(item),
                    BranchName = (string?)itemType.GetProperty("BranchName")?.GetValue(item),
                    TotalAmount = (decimal)itemType.GetProperty("TotalAmount")?.GetValue(item)!,
                    ChequeCount = (int)itemType.GetProperty("ChequeCount")?.GetValue(item)!
                });
            }

            return result;
        }

        public async Task<bool> MarkAsReturnAsync(long id, string userId)
        {
            return await _repository.MarkAsReturnAsync(id, userId);
        }

        private ChequeInfoDTO MapToDTO(ChequeInfo entity)
        {
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
                UpdatedBy = entity.UpdatedBy
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

    }
}
