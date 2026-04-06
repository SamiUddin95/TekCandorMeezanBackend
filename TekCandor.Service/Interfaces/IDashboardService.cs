using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface IDashboardService
    {
        Task<List<DashboardDTO>> GetDashboardAsync(string hubIds, string branchOrHub);
    }
}
