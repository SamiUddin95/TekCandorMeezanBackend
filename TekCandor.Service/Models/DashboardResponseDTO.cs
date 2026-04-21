using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class DashboardResponseDTO
    {
        public List<DashboardDTO> Normal { get; set; }
        public List<DashboardDTO> SameDay { get; set; }
        public int NormalTotalCount { get; set; }
        public decimal NomalTotalAmount { get; set; }

        public int SameDayTotalCount { get; set; }
        public decimal SameDayTotalAmount { get; set; }

        public int BothTotalCount { get; set; }
        public decimal BothTotalAmount { get; set; }
    }
}
