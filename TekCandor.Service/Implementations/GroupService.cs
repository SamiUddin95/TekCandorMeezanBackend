using System;
using System.Linq;
using System.Threading.Tasks;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;
using TekCandor.Repository.Entities;

namespace TekCandor.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;

        public GroupService(IGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<GroupDTO>> GetAllGroupsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = (await _repository.GetAllQueryableAsync()).Where(g => g.IsDeleted != true);

            var totalCount = query.Count();

            var items = query
                .OrderByDescending(g => g.UpdatedOn ?? g.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GroupDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    IsDeleted = g.IsDeleted,
                    CreatedBy = g.CreatedBy,
                    UpdatedBy = g.UpdatedBy,
                    CreatedOn = g.CreatedOn,
                    UpdatedOn = g.UpdatedOn
                });

            return new PagedResult<GroupDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<GroupDTO?> GetByIdAsync(long id)
        {
            var g = await _repository.GetByIdAsync(id);
            if (g == null || g.IsDeleted == true) return null;

            return new GroupDTO
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                IsDeleted = g.IsDeleted,
                CreatedBy = g.CreatedBy,
                UpdatedBy = g.UpdatedBy,
                CreatedOn = g.CreatedOn,
                UpdatedOn = g.UpdatedOn
            };
        }

        public async Task<GroupDTO> CreateGroupAsync(GroupDTO group)
        {
            var entity = new Group
            {
                Name = group.Name,
                Description = group.Description,
                IsDeleted = false,
                CreatedBy = group.CreatedBy,
                UpdatedBy = string.IsNullOrWhiteSpace(group.UpdatedBy) ? group.CreatedBy : group.UpdatedBy,
                CreatedOn = DateTime.Now,
                UpdatedOn = null
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return new GroupDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsDeleted = entity.IsDeleted,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task<GroupDTO?> UpdateAsync(GroupDTO group)
        {
            var existing = await _repository.GetByIdAsync(group.Id);
            if (existing == null || existing.IsDeleted == true)
                return null;

            existing.Name = group.Name;
            existing.Description = group.Description;
            existing.UpdatedBy = group.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;

            await _repository.SaveChangesAsync();

            return new GroupDTO
            {
                Id = existing.Id,
                Name = existing.Name,
                Description = existing.Description,
                IsDeleted = existing.IsDeleted,
                CreatedBy = existing.CreatedBy,
                UpdatedBy = existing.UpdatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedOn = existing.UpdatedOn
            };
        }

        public async Task<bool> SoftDeleteAsync(long id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null || existing.IsDeleted == true)
                return false;

            existing.IsDeleted = true;
            existing.UpdatedOn = DateTime.Now;

            return await _repository.SaveChangesAsync();
        }
    }
}
