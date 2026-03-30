using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public  class CBCReportDTO
    {
        public System.DateTime Date { get; set; }
        public string? CycleCode { get; set; }
        public string? HubCode { get; set; }
        public string? CoreFTId { get; set; }
        public decimal? Amount { get; set; }
        public string? ChequeNumber { get; set; }
        public string? SenderBranchCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? IBAN { get; set; }
        public string? AccountTitle { get; set; }
        public string? Remarks { get; set; }
        public string? BranchStaffId { get; set; }
        public string? CBCStatus { get; set; }


    }
}
