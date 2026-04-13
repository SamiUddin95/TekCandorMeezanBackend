using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponseDTO> GetDashboardAsync(string hubIds, string branchOrHub);
    }
}
