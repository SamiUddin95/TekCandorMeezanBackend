using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class ChequeInfoService : IChequeInfoService
    {
        private readonly IChequeInfoRepository _repository;

        public ChequeInfoService(IChequeInfoRepository repository)
        {
            _repository = repository;
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
                ChequeNo = dto.ChequeNo,
                PayingBankCode = dto.PayingBankCode,
                PayingBranchCode = dto.PayingBranchCode,
                Amount = dto.Amount,
                ChequeDate = dto.ChequeDate,
                InstrumentType = dto.InstrumentType,
                MICR = dto.MICR,
                OCREngine = dto.OCREngine,
                ProcessingTime = dto.ProcessingTime,
                Accuracy = dto.Accuracy,
                ImageF = dto.ImageF,
                ImageB = dto.ImageB,
                ImageU = dto.ImageU,
                Currency = dto.Currency,
                Remarks = dto.Remarks,
                BranchId = dto.BranchId,
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
            entity.BranchId = dto.BranchId;
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

        public async Task<List<ChequeInfoDTO>> GetByBranchIdAsync(long branchId)
        {
            var entities = await _repository.GetByBranchIdAsync(branchId);
            return entities.Select(MapToDTO).ToList();
        }

        public async Task<List<ChequeInfoDTO>> GetByStatusAsync(string status)
        {
            var entities = await _repository.GetByStatusAsync(status);
            return entities.Select(MapToDTO).ToList();
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
                BranchId = entity.BranchId,
                BranchName = entity.Branch?.Name,
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
    }
}
