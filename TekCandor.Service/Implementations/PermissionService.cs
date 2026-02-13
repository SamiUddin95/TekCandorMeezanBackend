using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Implementations;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<PermissionDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = (await _repository.GetAllQueryableAsync())
                        .Where(p => p.IsDeleted != true);

            var totalCount = query.Count();

            var items = query
                .OrderByDescending(p => p.UpdatedOn ?? p.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PermissionDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    IsDeleted = p.IsDeleted,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn
                });

            return new PagedResult<PermissionDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
