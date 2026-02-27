using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class CycleService : ICycleService
    {
        private readonly ICycleRepository _repository;

        public CycleService(ICycleRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<CycleDTO>> GetAllCyclesAsync(int pageNumber, int pageSize, string? name = null)
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

            var cycles = query
                .OrderByDescending(c => c.UpdatedOn ?? c.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = cycles.Select(c => new CycleDTO
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                IsDeleted = c.IsDeleted,
                CreatedBy = c.CreatedBy,
                UpdatedBy = c.UpdatedBy,
                CreatedOn = c.CreatedOn,
                UpdatedOn = c.UpdatedOn
            });

            return new PagedResult<CycleDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public CycleDTO CreateCycle(CycleDTO cycle)
        {
            var entity = new Cycle
            {
                Id = cycle.Id,
                Code = cycle.Code,
                Name = cycle.Name,
                Description = cycle.Description,
                IsDeleted = false,
                
                CreatedBy = cycle.CreatedBy,
                CreatedOn = DateTime.Now,

                UpdatedBy = null,      
                UpdatedOn = null       
            };

            var created = _repository.Add(entity);

            return new CycleDTO
            {
                Id = created.Id,
                Code = created.Code,
                Name = created.Name,
                IsDeleted = created.IsDeleted,

                CreatedBy = created.CreatedBy,
                CreatedOn = created.CreatedOn,

                UpdatedBy = created.UpdatedBy,   
                UpdatedOn = created.UpdatedOn    
            };
        }


        public CycleDTO? GetById(long id)
        {
            var c = _repository.GetById(id);
            if (c == null) return null;
            return new CycleDTO
            {
              
                Id=c.Id,
                Code = c.Code,
                Name = c.Name,
                IsDeleted = c.IsDeleted,
                CreatedBy = c.CreatedBy,
                UpdatedBy = c.UpdatedBy,
                CreatedOn = c.CreatedOn,
                UpdatedOn = c.UpdatedOn
            };
        }

        public CycleDTO? Update(CycleDTO cycle)
        {
            var entity = new Cycle
            {
                Id=cycle.Id,
                Code = cycle.Code,
                Name = cycle.Name,
                IsDeleted = cycle.IsDeleted,
                UpdatedBy = cycle.UpdatedBy,
                UpdatedOn = DateTime.Now,
                CreatedBy = cycle.CreatedBy,
                CreatedOn = cycle.CreatedOn
            };
            var updated = _repository.Update(entity);
            if (updated == null) return null;
            return new CycleDTO
            {
                Id=updated.Id,
                Code = updated.Code,
                Name = updated.Name,
                IsDeleted = updated.IsDeleted,
                CreatedBy = updated.CreatedBy,
                UpdatedBy = updated.UpdatedBy,
                CreatedOn = updated.CreatedOn,
                UpdatedOn = updated.UpdatedOn
            };
        }

        public bool SoftDelete(long id)
        {
            return _repository.SoftDelete(id);
        }
    }
}
