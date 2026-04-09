using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Models;

namespace TekCandor.Repository.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllQueryableAsync();
        Task<Group?> GetByIdAsync(long id);
        Task AddAsync(Group group);
        Task<bool> SaveChangesAsync();
        Task AssignPermissionsAsync(long groupId, List<long> permissionIds);
        Task<List<long>> GetGroupPermissionsAsync(long groupId);
        Task<List<PermissionDetailDTO>> GetGroupPermissionsWithDetailsAsync(long groupId);
        Task<List<UserDetailDTO>> GetGroupUsersAsync(long groupId);
        Task AddUsersToGroupAsync(long securityGroupId, List<long> userIds);
    }
}
