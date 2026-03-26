using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ReturnMemoReportDTO
    {
        public string? AccountNumber { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal? Amount { get; set; }
        public string? AccountTitle { get; set; }
        public string? CycleCode { get; set; }
        public string? Returnreasone { get; set; }
        public System.DateTime Date { get; set; }
        public string? SenderBranchCode { get; set; }



    }
}
