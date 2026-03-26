using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ClearingLogReportDTO
    {
        public string? CoreFTId { get; set; }

        public string? AccountNumber { get; set; }

        public string? OldAccount { get; set; }

        public decimal? Amount { get; set; }

        public string? ApproverId { get; set; }

        public string? ChequeNumber { get; set; }
        public string? AuthorizerId { get; set; }

        public string? ReceiverBankCode { get; set; }
        public string? ReceiverBranchCode { get; set; }
        public string? BranchStaffId { get; set; }
        public string? Remarks { get; set; }
        public string? CityCode { get; set; }
        public string? TrProcORRecTime { get; set; }
        public string? SenderBranchCode { get; set; }
        public string? TrRecTimeBranch { get; set; }

        public string? BranchRemarks { get; set; }





    }
}
