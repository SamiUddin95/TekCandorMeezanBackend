using System;
using System.Collections.Generic;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IHubService
    {
        
        Task<PagedResult<HubDTO>> GetAllHubsAsync(int pageNumber, int pageSize);

        Task<HubDTO?> GetByIdAsync(long id);
        Task<HubDTO> CreateHubAsync(HubDTO hub);
        Task<HubDTO?> UpdateAsync(HubDTO hub);
        Task<bool> SoftDeleteAsync(long id);

    }
}
