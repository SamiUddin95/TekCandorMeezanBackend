using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class DashboardDTO
    {
        public string? Status { get; set; }
        public int Cheques { get; set; }
        public decimal Amount { get; set; }
    }
}
