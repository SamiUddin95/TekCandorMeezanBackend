using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;

namespace TekCandor.Repository.Implementations
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllQueryableAsync()
        {
            return _context.Group.ToList();
        }

        public async Task<Group?> GetByIdAsync(long id)
        {
            return await _context.Group.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Group group)
        {
            await _context.Group.AddAsync(group);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AssignPermissionsAsync(long groupId, List<long> permissionIds)
        {
            // Remove existing permissions
            var existing = _context.SecurityGroup_PermissionRecord
                .Where(x => x.GroupId == groupId);

            _context.SecurityGroup_PermissionRecord.RemoveRange(existing);

            // Add new permissions
            var newRecords = permissionIds.Select(pid => new SecurityGroup_PermissionRecord
            {
                GroupId = groupId,
                PermissionId = pid
            });

            await _context.SecurityGroup_PermissionRecord.AddRangeAsync(newRecords);
            await _context.SaveChangesAsync();
        }

    }
}
