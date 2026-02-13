using System.Collections.Generic;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;

namespace TekCandor.Repository.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllQueryableAsync();
        Task<Group?> GetByIdAsync(long id);
        Task AddAsync(Group group);
        Task<bool> SaveChangesAsync();
        Task AssignPermissionsAsync(long groupId, List<long> permissionIds);

    }
}
