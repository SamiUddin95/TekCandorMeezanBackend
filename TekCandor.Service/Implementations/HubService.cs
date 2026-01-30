using System;
using System.Collections.Generic;
using System.Linq;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Repository.Entities;

namespace TekCandor.Service.Implementations
{
    public class HubService : IHubService
    {
        private readonly IHubRepository _repository;

        public HubService(IHubRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<HubDTO> GetAllHubs()
        {
            var entities = _repository.GetAll();
            return entities.Select(h => new HubDTO
            {
                Id = h.Id,
                Version = h.Version,
                Code = h.Code,
                Name = h.Name,
                IsDeleted = h.IsDeleted,
                IsNew = h.IsNew,
                CreatedUser = h.CreatedUser,
                CreatedDateTime = h.CreatedDateTime,
                ModifiedUser = h.ModifiedUser,
                ModifiesDateTime = h.ModifiesDateTime,
                CrAccSameDay = h.CrAccSameDay,
                CrAccNormal = h.CrAccNormal,
                CrAccIntercity = h.CrAccIntercity,
                CrAccDollar = h.CrAccDollar
            });
        }

        public HubDTO CreateHub(HubDTO hub)
        {
            var entity = new Hub
            {
                Version = hub.Version,
                Code = hub.Code,
                Name = hub.Name,
                IsDeleted = false,
                IsNew = hub.IsNew,
                CreatedUser = hub.CreatedUser,
                ModifiedUser = string.IsNullOrWhiteSpace(hub.ModifiedUser) ? hub.CreatedUser : hub.ModifiedUser,
                CreatedDateTime = DateTime.UtcNow,
                ModifiesDateTime = null,
                CrAccSameDay = hub.CrAccSameDay,
                CrAccNormal = hub.CrAccNormal,
                CrAccIntercity = hub.CrAccIntercity,
                CrAccDollar = hub.CrAccDollar
            };
            var created = _repository.Add(entity);
            return new HubDTO
            {
                Id = created.Id,
                Version = created.Version,
                Code = created.Code,
                Name = created.Name,
                IsDeleted = created.IsDeleted,
                IsNew = created.IsNew,
                CreatedUser = created.CreatedUser,
                ModifiedUser = created.ModifiedUser,
                CreatedDateTime = created.CreatedDateTime,
                ModifiesDateTime = created.ModifiesDateTime,
                CrAccSameDay = created.CrAccSameDay,
                CrAccNormal = created.CrAccNormal,
                CrAccIntercity = created.CrAccIntercity,
                CrAccDollar = created.CrAccDollar
            };
        }

        public HubDTO? GetById(Guid id)
        {
            var h = _repository.GetById(id);
            if (h == null) return null;
            return new HubDTO
            {
                Id = h.Id,
                Version = h.Version,
                Code = h.Code,
                Name = h.Name,
                IsDeleted = h.IsDeleted,
                IsNew = h.IsNew,
                CreatedUser = h.CreatedUser,
                ModifiedUser = h.ModifiedUser,
                CreatedDateTime = h.CreatedDateTime,
                ModifiesDateTime = h.ModifiesDateTime,
                CrAccSameDay = h.CrAccSameDay,
                CrAccNormal = h.CrAccNormal,
                CrAccIntercity = h.CrAccIntercity,
                CrAccDollar = h.CrAccDollar
            };
        }

        public HubDTO? Update(HubDTO hub)
        {
            var entity = new Hub
            {
                Id = hub.Id,
                Version = hub.Version,
                Code = hub.Code,
                Name = hub.Name,
                IsDeleted = hub.IsDeleted,
                IsNew = hub.IsNew,
                CreatedUser = hub.CreatedUser,
                CreatedDateTime = hub.CreatedDateTime,
                ModifiedUser = hub.ModifiedUser,
                ModifiesDateTime = DateTime.UtcNow,
                CrAccSameDay = hub.CrAccSameDay,
                CrAccNormal = hub.CrAccNormal,
                CrAccIntercity = hub.CrAccIntercity,
                CrAccDollar = hub.CrAccDollar
            };
            var updated = _repository.Update(entity);
            if (updated == null) return null;
            return new HubDTO
            {
                Id = updated.Id,
                Version = updated.Version,
                Code = updated.Code,
                Name = updated.Name,
                IsDeleted = updated.IsDeleted,
                IsNew = updated.IsNew,
                CreatedUser = updated.CreatedUser,
                ModifiedUser = updated.ModifiedUser,
                CreatedDateTime = updated.CreatedDateTime,
                ModifiesDateTime = updated.ModifiesDateTime,
                CrAccSameDay = updated.CrAccSameDay,
                CrAccNormal = updated.CrAccNormal,
                CrAccIntercity = updated.CrAccIntercity,
                CrAccDollar = updated.CrAccDollar
            };
        }

        public bool SoftDelete(Guid id)
        {
            return _repository.SoftDelete(id);
        }
    }
}
