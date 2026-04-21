using System;

namespace TekCandor.Service.Outward.Models
{
    public class ReturnDetailDTO
    {
        public string? BeneficiaryTitle { get; set; }
        public string? AccountNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? BranchName { get; set; }
        public string? ReturnReason { get; set; }
        public string? ChequeNo { get; set; }
        public decimal? Amount { get; set; }
        public string? ImageF { get; set; }
        public string? ImageB { get; set; }
        public string? ImageU { get; set; }
    }
}
