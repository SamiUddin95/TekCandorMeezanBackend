using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repository;
        public BranchService(IBranchRepository repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<BranchDTO>> GetBranchesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repository.GetAllQueryable()
                                   .Where(b => !b.IsDeleted);

            var totalCount = query.Count();

            var branches = query
               .OrderByDescending(h => h.UpdatedOn ?? h.CreatedOn)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            var dtos = branches.Select(b => new BranchDTO
            {
                Code = b.Code,
                NIFTBranchCode = b.NIFTBranchCode,
                Name = b.Name,
                HubId = b.HubId,
                Version = b.Version,
                IsDeleted = b.IsDeleted,
                CreatedBy = b.CreatedBy,
                CreatedOn = b.CreatedOn,
                UpdatedBy = b.UpdatedBy,
                UpdatedOn = b.UpdatedOn,
                Email1 = b.Email1,
                Email2 = b.Email2,
                Email3 = b.Email3
            });

            return new PagedResult<BranchDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<BranchDTO> CreateBranchAsync(BranchDTO branch)
        {
            var entity = new Branch
            {
                Code = branch.Code,
                NIFTBranchCode = branch.NIFTBranchCode,
                Name = branch.Name,
                HubId = branch.HubId,
                Version = branch.Version,
                IsDeleted = false,
                CreatedBy = branch.CreatedBy,
                CreatedOn = DateTime.Now,
                Email1 = branch.Email1,
                Email2 = branch.Email2,
                Email3 = branch.Email3
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return new BranchDTO
            {
                Code = entity.Code,
                NIFTBranchCode = entity.NIFTBranchCode,
                Name = entity.Name,
                HubId = entity.HubId,
                Version = entity.Version,
                IsDeleted = entity.IsDeleted,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                Email1 = entity.Email1,
                Email2 = entity.Email2,
                Email3 = entity.Email3
            };
        }


        public async Task<BranchDTO?> GetByIdAsync(long id)
        {
            var b = await _repository.GetByIdAsync(id);
            if (b == null || b.IsDeleted) return null;

            return new BranchDTO
            {
                
                Code = b.Code,
                NIFTBranchCode = b.NIFTBranchCode,
                Name = b.Name,
                HubId = b.HubId,
                Version = b.Version,
                IsDeleted = b.IsDeleted,
                CreatedBy = b.CreatedBy,
                CreatedOn = b.CreatedOn,
                UpdatedBy = b.UpdatedBy,
                UpdatedOn = b.UpdatedOn,
                Email1 = b.Email1,
                Email2 = b.Email2,
                Email3 = b.Email3
            };
        }

        public async Task<BranchDTO?> UpdateBranchAsync(BranchDTO branch)
        {
            var existing = await _repository.GetByIdAsync(branch.Id);
            if (existing == null || existing.IsDeleted) return null;

            existing.Code = branch.Code;
            existing.NIFTBranchCode = branch.NIFTBranchCode;
            existing.Name = branch.Name;
            existing.HubId = branch.HubId;
            existing.Version = branch.Version;
            existing.UpdatedBy = branch.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;
            existing.Email1 = branch.Email1;
            existing.Email2 = branch.Email2;
            existing.Email3 = branch.Email3;

            await _repository.SaveChangesAsync();

            return new BranchDTO
            {
              
                Code = existing.Code,
                NIFTBranchCode = existing.NIFTBranchCode,
                Name = existing.Name,
                HubId = existing.HubId,
                Version = existing.Version,
                IsDeleted = existing.IsDeleted,
                CreatedBy = existing.CreatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedBy = existing.UpdatedBy,
                UpdatedOn = existing.UpdatedOn,
                Email1 = existing.Email1,
                Email2 = existing.Email2,
                Email3 = existing.Email3
            };
        }


        public async Task<bool> SoftDeleteAsync(long id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null || existing.IsDeleted) return false;

            existing.IsDeleted = true;
            existing.UpdatedOn = DateTime.Now;

            return await _repository.SaveChangesAsync();
        }


    }

}



