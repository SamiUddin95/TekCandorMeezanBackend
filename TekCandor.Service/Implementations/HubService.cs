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
    public class HubService : IHubService
    {
        private readonly IHubRepository _repository;

        public HubService(IHubRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<HubDTO>> GetAllHubsAsync(
    int pageNumber,
    int pageSize,
    string? name = null
)
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

            var items = await query
                .OrderByDescending(h => h.UpdatedOn ?? h.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new HubDTO
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    IsDeleted = h.IsDeleted,
                    CreatedBy = h.CreatedBy,
                    CreatedOn = h.CreatedOn,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedOn = h.UpdatedOn,
                    CrAccSameDay = h.CrAccSameDay,
                    CrAccNormal = h.CrAccNormal,
                    CrAccIntercity = h.CrAccIntercity,
                    CrAccDollar = h.CrAccDollar
                })
                .ToListAsync();

            return new PagedResult<HubDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public HubDTO CreateHub(HubDTO hub)
        {
            var entity = new Hub
            {
                Id = hub.Id,
                Code = hub.Code,
                Name = hub.Name,
                IsDeleted = false,

                CreatedBy = hub.CreatedBy,
                CreatedOn = DateTime.Now,

                UpdatedBy = null,    
                UpdatedOn = null, 

                CrAccSameDay = hub.CrAccSameDay,
                CrAccNormal = hub.CrAccNormal,
                CrAccIntercity = hub.CrAccIntercity,
                CrAccDollar = hub.CrAccDollar
            };

            var created = _repository.Add(entity);

            return new HubDTO
            {
                Id = created.Id,
                Code = created.Code,
                Name = created.Name,
                IsDeleted = created.IsDeleted,

                CreatedBy = created.CreatedBy,
                CreatedOn = created.CreatedOn,

                UpdatedBy = null,
                UpdatedOn = null,   

                CrAccSameDay = created.CrAccSameDay,
                CrAccNormal = created.CrAccNormal,
                CrAccIntercity = created.CrAccIntercity,
                CrAccDollar = created.CrAccDollar
            };
        }


        public async Task<HubDTO?> GetByIdAsync(long id)
        {
            var h = await _repository.GetByIdAsync(id);
            if (h == null || h.IsDeleted) return null;

            return new HubDTO
            {
                Id = h.Id,
                Code = h.Code,
                Name = h.Name,
                IsDeleted = h.IsDeleted,
                CreatedBy = h.CreatedBy,
                UpdatedBy = h.UpdatedBy,
                CreatedOn = h.CreatedOn,
                UpdatedOn = h.UpdatedOn,
                CrAccSameDay = h.CrAccSameDay,
                CrAccNormal = h.CrAccNormal,
                CrAccIntercity = h.CrAccIntercity,
                CrAccDollar = h.CrAccDollar
            };
        }


        public async Task<HubDTO?> UpdateAsync(HubDTO hub)
        {
            var existing = await _repository.GetByIdAsync(hub.Id);
            if (existing == null || existing.IsDeleted)
                return null;
           
            existing.Code = hub.Code;
            existing.Name = hub.Name;
            existing.UpdatedBy = hub.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;
            existing.CrAccSameDay = hub.CrAccSameDay;
            existing.CrAccNormal = hub.CrAccNormal;
            existing.CrAccIntercity = hub.CrAccIntercity;
            existing.CrAccDollar = hub.CrAccDollar;

            await _repository.SaveChangesAsync();

            return new HubDTO
            { 
                Id = existing.Id,
                Code = existing.Code,
                Name = existing.Name,
                IsDeleted = existing.IsDeleted,
                CreatedBy = existing.CreatedBy,
                UpdatedBy = existing.UpdatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedOn = existing.UpdatedOn,
                CrAccSameDay = existing.CrAccSameDay,
                CrAccNormal = existing.CrAccNormal,
                CrAccIntercity = existing.CrAccIntercity,
                CrAccDollar = existing.CrAccDollar
            };
        }


        public async Task<bool> SoftDeleteAsync(long id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null || existing.IsDeleted)
                return false;

            existing.IsDeleted = true;
            existing.UpdatedOn = DateTime.Now;

            return await _repository.SaveChangesAsync();
        }

    }
}
