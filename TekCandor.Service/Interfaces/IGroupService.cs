using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IGroupService
    {
        Task<PagedResult<GroupDTO>> GetAllGroupsAsync(int pageNumber, int pageSize);
        Task<GroupDTO?> GetByIdAsync(long id);
        Task<GroupDTO> CreateGroupAsync(GroupDTO group);
        Task<GroupDTO?> UpdateAsync(GroupDTO group);
        Task<bool> SoftDeleteAsync(long id);
    }
}
