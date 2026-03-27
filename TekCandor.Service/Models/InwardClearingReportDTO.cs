using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class InwardClearingReportDTO
    {
        public string? CoreFTId { get; set; }

        public string? AccountNumber { get; set; }

        public string? OldAccount { get; set; }

        public decimal? Amount { get; set; }

        public string? ApproverId { get; set; }

        public string? ChequeNumber { get; set; }

        public string? AuthorizerId { get; set; }

        public string? TrProcORRecTime { get; set; }

    }
}
