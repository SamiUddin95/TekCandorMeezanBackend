using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class BranchWiseReportDTO
    {
        public string? Date { get; set; }
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountTitle { get; set; }
        public decimal? Amount { get; set; }
        public string? ReturnReason { get; set; }
        public string? HubCode { get; set; }
        public string? TransactionCode { get; set; }
        
    }
}

