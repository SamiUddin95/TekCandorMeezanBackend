using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Entities.Outward;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class BusinessDateService : IBusinessDateService
    {
        private readonly IBusinessDateRepository _repository;

        public BusinessDateService(IBusinessDateRepository repository)
        {
            _repository = repository;
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
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new BusinessDateDTO
            {
                Id = e.Id,
                BusinessDate = e.BusinessDate1,
                IsActive = e.IsActive,
                StartedBy = e.StartedBy,
                StartedAt = e.StartedAt
            }).ToList();
        }

    }
}
