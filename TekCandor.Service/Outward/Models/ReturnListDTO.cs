using System;

namespace TekCandor.Service.Outward.Models
{
    public class ReturnListDTO
    {
        public long ChequeInfoId { get; set; }
        public DateTime? Date { get; set; }
        public string? DepositorType { get; set; }
        public string? AccountNo { get; set; }
        public string? CNIC { get; set; }
        public string? DepositorTitle { get; set; }
        public string? BranchName { get; set; }
        public string? ChequeNo { get; set; }
        public decimal? Amount { get; set; }
        public string? MICR { get; set; }
        public string? Status { get; set; }
        public string? MatchStatus { get; set; }
        public long NiftStagingId { get; set; }
        public string? FileName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? ReturnCode { get; set; }
        public string? ReturnReason { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
