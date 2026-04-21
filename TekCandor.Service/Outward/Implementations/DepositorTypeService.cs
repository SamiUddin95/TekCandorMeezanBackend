using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Interfaces.Outward;
using TekCandor.Service.Outward.Interfaces;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Implementations
{
    public class DepositorTypeService : IDepositorTypeService
    {
        private readonly IDepositorTypeRepository _repository;

        public DepositorTypeService(IDepositorTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepositorTypeDTO>> GetAllAsync()
        {
            var depositorTypes = await _repository.GetAllAsync();

            return depositorTypes.Select(d => new DepositorTypeDTO
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();
        }
    }
}
