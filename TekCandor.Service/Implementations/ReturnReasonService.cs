using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedResult<ReturnReasonDTO>> GetAll(int pageNumber, int pageSize, string? name = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = await _repository.GetAllQueryableAsync();

            query = query.Where(h => !h.IsDeleted);
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(h => h.Name.Contains(name.Trim()));
            }

            var totalCount = await query.CountAsync();

            var returnReasons = await query
                .OrderByDescending(r => r.UpdatedOn ?? r.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = returnReasons.Select(r => new ReturnReasonDTO
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
            });

            return new PagedResult<ReturnReasonDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
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
                Code = dto.Code,
                AlphaReturnCodes = dto.AlphaReturnCodes,
                NumericReturnCodes = dto.NumericReturnCodes,
                DescriptionWithReturnCodes = dto.DescriptionWithReturnCodes,
                DefaultCallBack = dto.DefaultCallBack,
                Name = dto.Name,
                IsDeleted = false,

                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.Now,

                UpdatedBy = null,
                UpdatedOn = null
            };

           
            var createdEntity = _repository.Add(entity);

      
            return new ReturnReasonDTO
            {
                Id = createdEntity.Id,
                Code = createdEntity.Code,
                AlphaReturnCodes = createdEntity.AlphaReturnCodes,
                NumericReturnCodes = createdEntity.NumericReturnCodes,
                DescriptionWithReturnCodes = createdEntity.DescriptionWithReturnCodes,
                DefaultCallBack = createdEntity.DefaultCallBack,
                Name = createdEntity.Name,
                IsDeleted = createdEntity.IsDeleted,
                CreatedBy = createdEntity.CreatedBy,
                CreatedOn = createdEntity.CreatedOn,
                UpdatedBy = createdEntity.UpdatedBy,
                UpdatedOn = createdEntity.UpdatedOn
            };
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
