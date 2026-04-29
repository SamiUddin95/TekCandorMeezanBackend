using System;

namespace TekCandor.Service.Outward.Models
{
    public class ChequeInfoDTO
    {
        public long Id { get; set; }
        public DateTime? Date { get; set; }
        public string? DepositorType { get; set; }
        public string? AccountNo { get; set; }
        public string? CNIC { get; set; }
        public string? DepositorTitle { get; set; }
        public string? BeneficiaryAccountNumber { get; set; }
        public string? BeneficiaryTitle { get; set; }
        public string? AccountStatus { get; set; }
        public string? BeneficiaryBranchCode { get; set; }
        public string? ChequeNo { get; set; }
        public string? PayingBankCode { get; set; }
        public string? PayingBranchCode { get; set; }
        public decimal? Amount { get; set; }
        public string? ChequeDate { get; set; }
        public string? InstrumentType { get; set; }
        public string? MICR { get; set; }
        public string? OCREngine { get; set; }
        public string? ProcessingTime { get; set; }
        public string? Accuracy { get; set; }
        public string? ImageF { get; set; }
        public string? ImageB { get; set; }
        public string? ImageU { get; set; }
        public string? Currency { get; set; }
        public string? Remarks { get; set; }
        public string? ReceiverBranchCode { get; set; }
        public string? BranchName { get; set; }
        public string? DrawerBank { get; set; }
        public string? AmountInWords { get; set; }
        public string? ReferenceNo { get; set; }
        public long? DepositSlipId { get; set; }
        public string? Status { get; set; }
        public bool? IsReconciled { get; set; }
        public bool? IsReturned { get; set; }
        public bool? IsRealized { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Hubcode { get; set; }
        public string? BatchId { get; set; }
    }
}
