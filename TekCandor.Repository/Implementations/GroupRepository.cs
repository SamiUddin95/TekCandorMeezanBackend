using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Models;

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

        public async Task<List<long>> GetGroupPermissionsAsync(long groupId)
        {
            var permissions = await _context.SecurityGroup_PermissionRecord
                .AsNoTracking()
                .Where(x => x.GroupId == groupId)
                .Select(x => x.PermissionId)
                .ToListAsync();
            
            return permissions ?? new List<long>();
        }

        public async Task<List<PermissionDetailDTO>> GetGroupPermissionsWithDetailsAsync(long groupId)
        {
            var permissions = await (from sgp in _context.SecurityGroup_PermissionRecord
                                     join p in _context.Permission on sgp.PermissionId equals p.Id
                                     where sgp.GroupId == groupId && p.IsDeleted == false
                                     select new PermissionDetailDTO
                                     {
                                         Id = p.Id,
                                         Name = p.Name,
                                         Description = p.Description,
                                         IsDeleted = p.IsDeleted,
                                         CreatedOn = p.CreatedOn,
                                         UpdatedOn = p.UpdatedOn
                                     })
                                     .AsNoTracking()
                                     .ToListAsync();

            return permissions ?? new List<PermissionDetailDTO>();
        }

        public async Task<List<UserDetailDTO>> GetGroupUsersAsync(long groupId)
        {
            var users = await (from sgu in _context.SecurityGroup_User
                               join u in _context.Users on sgu.UserId equals u.Id
                               where sgu.SecurityGroupId == groupId && u.IsDeleted != true
                               select new UserDetailDTO
                               {
                                   Id = u.Id,
                                   LoginName = u.LoginName,
                                   Name = u.Name,
                                   Email = u.Email
                               })
                               .AsNoTracking()
                               .ToListAsync();

            return users ?? new List<UserDetailDTO>();
        }

        public async Task AddUsersToGroupAsync(long securityGroupId, List<long> userIds)
        {
            var existingUserIds = await _context.SecurityGroup_User
                .Where(x => x.SecurityGroupId == securityGroupId)
                .Select(x => x.UserId)
                .ToListAsync();

            var newUserIds = userIds.Where(id => !existingUserIds.Contains(id)).ToList();

            if (newUserIds.Any())
            {
                var newRecords = newUserIds.Select(userId => new SecurityGroup_User
                {
                    SecurityGroupId = securityGroupId,
                    UserId = userId
                });

                await _context.SecurityGroup_User.AddRangeAsync(newRecords);
                await _context.SaveChangesAsync();
            }
        }

    }
}
