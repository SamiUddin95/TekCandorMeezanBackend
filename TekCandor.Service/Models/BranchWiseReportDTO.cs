using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class BranchWiseReportDTO
    {
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountTitle { get; set; }
        public decimal? Amount { get; set; }
        public string? ReturnReason { get; set; }
        public string? HubCode { get; set; }
        public string? CycleCode { get; set; }
        //public string? SenderBankCode { get; set; }
        //public string? SenderBranchCode { get; set; }
        //public string? ReceiverBankCode { get; set; }
        //public string? ReceiverBranchCode { get; set; }
        //public string? TransactionCode { get; set; }
        //public DateTime? Date { get; set; }
        //public string? code { get; set; }
        //public string? TrProcORRecTime { get; set; }

    }
}

