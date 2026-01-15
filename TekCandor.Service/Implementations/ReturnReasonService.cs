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
                Id = r.Id,
                Code = r.Code,
                AlphaReturnCodes = r.AlphaReturnCodes,
                NumericReturnCodes = r.NumericReturnCodes,
                DescriptionWithReturnCodes = r.DescriptionWithReturnCodes,
                DefaultCallBack = r.DefaultCallBack,
                Version = r.Version,
                Name = r.Name,
                IsNew = r.IsNew,
                IsDeleted = r.IsDeleted,
                CreatedUser = r.CreatedUser,
                CreatedDateTime = r.CreatedDateTime,
                ModifiedUser = r.ModifiedUser,
                ModifiedDateTime = r.ModifiedDateTime
            });
        }

        public ReturnReasonDTO? GetById(Guid id)
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
                Version = r.Version,
                Name = r.Name,
                IsNew = r.IsNew,
                IsDeleted = r.IsDeleted,
                CreatedUser = r.CreatedUser,
                CreatedDateTime = r.CreatedDateTime,
                ModifiedUser = r.ModifiedUser,
                ModifiedDateTime = r.ModifiedDateTime
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
                Version = dto.Version,
                Name = dto.Name,
                IsNew = dto.IsNew,
                IsDeleted = dto.IsDeleted,
                CreatedUser = dto.CreatedUser,
                CreatedDateTime = dto.CreatedDateTime,
                ModifiedUser = dto.ModifiedUser,
                ModifiedDateTime = dto.ModifiedDateTime
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
                Version = dto.Version,
                Name = dto.Name,
                IsNew = dto.IsNew,
                IsDeleted = dto.IsDeleted,
                CreatedUser = dto.CreatedUser,
                CreatedDateTime = dto.CreatedDateTime,
                ModifiedUser = dto.ModifiedUser,
                ModifiedDateTime = dto.ModifiedDateTime
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
                Version = updated.Version,
                Name = updated.Name,
                IsNew = updated.IsNew,
                IsDeleted = updated.IsDeleted,
                CreatedUser = updated.CreatedUser,
                CreatedDateTime = updated.CreatedDateTime,
                ModifiedUser = updated.ModifiedUser,
                ModifiedDateTime = updated.ModifiedDateTime
            };
        }

        public bool SoftDelete(Guid id)
        {
            return _repository.SoftDelete(id);
        }
    }
}
