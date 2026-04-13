using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class DashboardResponseDTO
    {
        public List<DashboardDTO> Normal { get; set; }
        public List<DashboardDTO> SameDay { get; set; }
    }
}
