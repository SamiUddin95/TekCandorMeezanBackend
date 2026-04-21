using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class BusinessDateService : IBusinessDateService
    {
        private readonly IBusinessDateRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;
        public BusinessDateService(IBusinessDateRepository repository,IMemoryCache cache, AppDbContext context)
        {
            _repository = repository;
            _cache = cache;
            _context = context;
        }

        public async Task<BusinessDateDTO> CreateAsync(BusinessDateDTO dto)
        {
            var entity = new BusinessDate
            {
                BusinessDate1 = dto.BusinessDate,
                IsActive = dto.IsActive,
                StartedBy = dto.StartedBy
            };

            var created = await _repository.CreateAsync(entity);

            return new BusinessDateDTO
            {
                Id = created.Id,
                BusinessDate = created.BusinessDate1,
                IsActive = created.IsActive,
                StartedBy = created.StartedBy,
                StartedAt = created.StartedAt
            };
        }

        public async Task<List<BusinessDateDTO>> GetAllAsync()
        {
            var cacheKey = "BusinessDate_Today";


            if (!_cache.TryGetValue(cacheKey, out List<BusinessDateDTO> cachedData))
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                var entities = await _context.BusinessDate
                    .Where(e => e.BusinessDate1 >= today && e.BusinessDate1 < tomorrow)
                    .ToListAsync();

                cachedData = entities.Select(e => new BusinessDateDTO
                {
                    Id = e.Id,
                    BusinessDate = e.BusinessDate1,
                    IsActive = e.IsActive,
                    StartedBy = e.StartedBy,
                    StartedAt = e.StartedAt
                }).ToList();

                // Cache for 5 minutes (adjust as needed)
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, cachedData, cacheOptions);
            }
            else if (cachedData.Count() == 0)
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                var entities = await _context.BusinessDate
                    .Where(e => e.BusinessDate1 >= today && e.BusinessDate1 < tomorrow)
                    .ToListAsync();

                cachedData = entities.Select(e => new BusinessDateDTO
                {
                    Id = e.Id,
                    BusinessDate = e.BusinessDate1,
                    IsActive = e.IsActive,
                    StartedBy = e.StartedBy,
                    StartedAt = e.StartedAt
                }).ToList();

                // Cache for 5 minutes (adjust as needed)
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, cachedData, cacheOptions);
            }

            return cachedData;
        }

    }
}
