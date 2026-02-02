using System;
using System.Collections.Generic;
using System.Linq;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class ReturnReasonService : IReturnReasonService
    {
        private readonly IReturnReasonRepository _repository;
        public ReturnReasonService(IReturnReasonRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ReturnReasonDTO> GetAll()
        {
            return _repository.GetAll().Select(r => new ReturnReasonDTO
            {
                Code = r.Code,
                AlphaReturnCodes = r.AlphaReturnCodes,
                NumericReturnCodes = r.NumericReturnCodes,
                DescriptionWithReturnCodes = r.DescriptionWithReturnCodes,
                DefaultCallBack = r.DefaultCallBack,
                Name = r.Name,
                IsDeleted = r.IsDeleted,
                CreatedBy = r.CreatedBy,
                CreatedOn = r.CreatedOn,
                UpdatedBy = r.UpdatedBy,
                UpdatedOn = r.UpdatedOn
            });
        }

        public ReturnReasonDTO? GetById(long id)
        {
            var r = _repository.GetById(id);
            if (r == null) return null;

            return new ReturnReasonDTO
            {
                Id = r.Id,
                Code = r.Code,
                AlphaReturnCodes = r.AlphaReturnCodes,
                NumericReturnCodes = r.NumericReturnCodes,
                DescriptionWithReturnCodes = r.DescriptionWithReturnCodes,
                DefaultCallBack = r.DefaultCallBack,
                Name = r.Name,
                IsDeleted = r.IsDeleted,
                CreatedBy = r.CreatedBy,
                CreatedOn = r.CreatedOn,
                UpdatedBy = r.UpdatedBy,
                UpdatedOn = r.UpdatedOn
            };
        }

        public ReturnReasonDTO Create(ReturnReasonDTO dto)
        {
            var entity = new ReturnReason
            {
                Id = dto.Id,
                Code = dto.Code,
                AlphaReturnCodes = dto.AlphaReturnCodes,
                NumericReturnCodes = dto.NumericReturnCodes,
                DescriptionWithReturnCodes = dto.DescriptionWithReturnCodes,
                DefaultCallBack = dto.DefaultCallBack,
                Name = dto.Name,
                IsDeleted = dto.IsDeleted,
                CreatedBy = dto.CreatedBy,
                CreatedOn = dto.CreatedOn,
                UpdatedBy = dto.UpdatedBy,
                UpdatedOn = dto.UpdatedOn
            };

            var created = _repository.Add(entity);
            dto.Id = created.Id;
            return dto;
        }

        public ReturnReasonDTO? Update(ReturnReasonDTO dto)
        {
            var entity = new ReturnReason
            {
                Id = dto.Id,
                Code = dto.Code,
                AlphaReturnCodes = dto.AlphaReturnCodes,
                NumericReturnCodes = dto.NumericReturnCodes,
                DescriptionWithReturnCodes = dto.DescriptionWithReturnCodes,
                DefaultCallBack = dto.DefaultCallBack,
                Name = dto.Name,
                IsDeleted = dto.IsDeleted,
                CreatedBy = dto.CreatedBy,
                CreatedOn = dto.CreatedOn,
                UpdatedBy = dto.UpdatedBy,
                UpdatedOn = dto.UpdatedOn
            };

            var updated = _repository.Update(entity);
            if (updated == null) return null;

            return new ReturnReasonDTO
            {
                Id = updated.Id,
                Code = updated.Code,
                AlphaReturnCodes = updated.AlphaReturnCodes,
                NumericReturnCodes = updated.NumericReturnCodes,
                DescriptionWithReturnCodes = updated.DescriptionWithReturnCodes,
                DefaultCallBack = updated.DefaultCallBack,
                Name = updated.Name,
                IsDeleted = updated.IsDeleted,
                CreatedBy = updated.CreatedBy,
                CreatedOn = updated.CreatedOn,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn
            };
        }

        public bool SoftDelete(long id)
        {
            return _repository.SoftDelete(id);
        }
    }
}
