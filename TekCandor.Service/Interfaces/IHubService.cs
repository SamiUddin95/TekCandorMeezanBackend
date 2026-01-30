using System;
using System.Collections.Generic;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IHubService
    {
        IEnumerable<HubDTO> GetAllHubs();
        HubDTO CreateHub(HubDTO hub);
        HubDTO? GetById(Guid id);
        HubDTO? Update(HubDTO hub);
        bool SoftDelete(Guid id);
    }
}
