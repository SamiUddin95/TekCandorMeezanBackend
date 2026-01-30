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
        public IEnumerable<BranchDTO> GetBranches()
        {
            var entities = _repository.GetAll();
            return entities.Select(e => new BranchDTO
            {
                Id = e.Id,
                NIFT = e.NIFT,
                Code = e.Code,
                NIFTBranchCode = e.NIFTBranchCode,
                Name = e.Name,
                HubId = e.HubId,
                Version = e.Version,
                IsNew = e.IsNew,
                IsDeleted = e.IsDeleted,
                CreatedUser = e.CreatedUser,
                CreatedDateTime = e.CreatedDateTime,
                ModifiedUser = e.ModifiedUser,
                ModifiedDateTime = e.ModifiedDateTime,
                Email1 = e.Email1,
                Email2 = e.Email2,
                Email3 = e.Email3
            });
        }
        public BranchDTO CreateBranch(BranchDTO branch)
        {
            var entity = new Branch
            {
                Id = branch.Id,
                NIFT = branch.NIFT,
                Code = branch.Code,
                NIFTBranchCode = branch.NIFTBranchCode,
                Name = branch.Name,
                HubId = branch.HubId,
                Version = branch.Version,
                IsNew = branch.IsNew,
                IsDeleted = branch.IsDeleted,
                CreatedUser = branch.CreatedUser,
                CreatedDateTime = branch.CreatedDateTime,
                ModifiedUser = branch.ModifiedUser,
                ModifiedDateTime = branch.ModifiedDateTime,
                Email1 = branch.Email1,
                Email2 = branch.Email2,
                Email3 = branch.Email3
            };
            var created = _repository.Add(entity);
            return new BranchDTO
            {
                Id = branch.Id,
                NIFT = branch.NIFT,
                Code = branch.Code,
                NIFTBranchCode = branch.NIFTBranchCode,
                Name = branch.Name,
                HubId = branch.HubId,
                Version = branch.Version,
                IsNew = branch.IsNew,
                IsDeleted = branch.IsDeleted,
                CreatedUser = branch.CreatedUser,
                CreatedDateTime = branch.CreatedDateTime,
                ModifiedUser = branch.ModifiedUser,
                ModifiedDateTime = branch.ModifiedDateTime,
                Email1 = branch.Email1,
                Email2 = branch.Email2,
                Email3 = branch.Email3,
            };

        }

        public BranchDTO? GetById(Guid Id)
        {
            var b = _repository.GetById(Id);
            if (b == null) return null;
            return new BranchDTO
            {
                Id = b.Id,
                NIFT = b.NIFT,
                Code = b.Code,
                NIFTBranchCode = b.NIFTBranchCode,
                Name = b.Name,
                HubId = b.HubId,
                Version = b.Version,
                IsNew = b.IsNew,
                IsDeleted = b.IsDeleted,
                CreatedUser = b.CreatedUser,
                CreatedDateTime = b.CreatedDateTime,
                ModifiedUser = b.ModifiedUser,
                ModifiedDateTime = b.ModifiedDateTime,
                Email1 = b.Email1,
                Email2 = b.Email2,
                Email3 = b.Email3
            };
        }
        public BranchDTO? UpdateBranch(BranchDTO branch)
        {
            var entity = new Branch
            {
                Id = branch.Id,
                NIFT = branch.NIFT,
                Code = branch.Code,
                NIFTBranchCode = branch.NIFTBranchCode,
                Name = branch.Name,
                HubId = branch.HubId,
                Version = branch.Version,
                IsNew = branch.IsNew,
                IsDeleted = branch.IsDeleted,
                CreatedUser = branch.CreatedUser,
                CreatedDateTime = branch.CreatedDateTime,
                ModifiedUser = branch.ModifiedUser,
                ModifiedDateTime = branch.ModifiedDateTime,
                Email1 = branch.Email1,
                Email2 = branch.Email2,
                Email3 = branch.Email3
            };
            var updated = _repository.Update(entity);
            if (updated == null) return null;
            return new BranchDTO
            {
                Id = updated.Id,
                NIFT = updated.NIFT,
                Code = updated.Code,
                NIFTBranchCode = updated.NIFTBranchCode,
                Name = updated.Name,
                HubId = updated.HubId,
                Version = updated.Version,
                IsNew = updated.IsNew,
                IsDeleted = updated.IsDeleted,
                CreatedUser = updated.CreatedUser,
                CreatedDateTime = updated.CreatedDateTime,
                ModifiedUser = updated.ModifiedUser,
                ModifiedDateTime = updated.ModifiedDateTime,
                Email1 = updated.Email1,
                Email2 = updated.Email2,
                Email3 = updated.Email3
            };

        }

        public bool SoftDelete(Guid id)
        {
            return _repository.SoftDelete(id);
        }

    }

}



