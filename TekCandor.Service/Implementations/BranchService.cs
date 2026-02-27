
using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Repository.Entities;

namespace TekCandor.Service.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _repository;

        public BranchService(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<BranchDTO>> GetAllBranchesAsync(int pageNumber, int pageSize, string? name = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = await _repository.GetAllQueryableAsync();
            query = query.Where(b => !b.IsDeleted);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(b => b.Name.Contains(name.Trim()));
            }

            var totalCount = await query.CountAsync();
            var branches = query
                .OrderByDescending(b => b.UpdatedOn ?? b.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = branches.Select(b => new BranchDTO
            {
                Id = b.Id,
                Code = b.Code,
                NIFTBranchCode = b.NIFTBranchCode,
                Name = b.Name,
                HubId = b.HubId,
                IsDeleted = b.IsDeleted,
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

        
        public bool CreateBranch(BranchDTO branch)
        {
            var entity = new Branch
            {
                Code = branch.Code,
                NIFTBranchCode = branch.NIFTBranchCode,
                Name = branch.Name,
                HubId = branch.HubId,
                IsDeleted = false,
                CreatedBy = branch.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = null,
                Email1 = branch.Email1,
                Email2 = branch.Email2,
                Email3 = branch.Email3,
                Version = 1,
                IsNew = false
            };

            _repository.Add(entity);

            return true;
        }

        public BranchDTO? GetById(long id)
        {
            var b = _repository.GetById(id);
            if (b == null) return null;

            return new BranchDTO
            {
                Id = b.Id,
                Code = b.Code,
                NIFTBranchCode = b.NIFTBranchCode,
                Name = b.Name,
                HubId = b.HubId,
                IsDeleted = b.IsDeleted,
                CreatedBy = b.CreatedBy,
                UpdatedBy = b.UpdatedBy,
                CreatedOn = b.CreatedOn,
                UpdatedOn = b.UpdatedOn,
                Email1 = b.Email1,
                Email2 = b.Email2,
                Email3 = b.Email3
            };
        }

        public BranchDTO? Update(BranchDTO branch)
        {
            var entity = new Branch
            {
                Id = branch.Id,
                Code = branch.Code,
                NIFTBranchCode = branch.NIFTBranchCode,
                Name = branch.Name,
                HubId = branch.HubId,
                IsDeleted = branch.IsDeleted,
                CreatedBy = branch.CreatedBy,
                CreatedOn = branch.CreatedOn,
                UpdatedBy = branch.UpdatedBy,
                UpdatedOn = DateTime.Now,
                Email1 = branch.Email1,
                Email2 = branch.Email2,
                Email3 = branch.Email3
            };

            var updated = _repository.Update(entity);
            if (updated == null) return null;

            return new BranchDTO
            {
                Id = updated.Id,
                Code = updated.Code,
                NIFTBranchCode = updated.NIFTBranchCode,
                Name = updated.Name,
                HubId = updated.HubId,
                IsDeleted = updated.IsDeleted,
                CreatedBy = updated.CreatedBy,
                CreatedOn = updated.CreatedOn,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn,
                Email1 = updated.Email1,
                Email2 = updated.Email2,
                Email3 = updated.Email3
            };
        }

        public bool SoftDelete(long id)
        {
            return _repository.SoftDelete(id);
        }
    }
}

