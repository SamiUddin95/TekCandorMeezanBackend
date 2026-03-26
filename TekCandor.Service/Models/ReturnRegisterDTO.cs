using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ReturnRegisterDTO
    {
        public string? AccountNumber { get; set; }
        public string? ChequeNumber { get; set; }

        public decimal? Amount { get; set; }

        public string? AccountTitle { get; set; }

        public string? ReceiverBranchCode { get; set; }

        public string? CycleCode { get; set; }

        public string? SenderBankCode { get; set; }

        public string? ApproverId { get; set; }

        public string? CoreFTId { get; set; }

        public string? TrProcORRecTime { get; set; }


    }
}
