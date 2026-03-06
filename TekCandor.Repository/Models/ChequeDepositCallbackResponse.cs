using System;

namespace TekCandor.Repository.Models
{
    public class ChequeDepositCallbackResponse
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Call Back";
        public string ImgF { get; set; } = string.Empty;
        public string ImgR { get; set; } = string.Empty;
        public string ImgU { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountTitle { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string AccountBalance { get; set; } = string.Empty;
        public string PoStatus { get; set; } = string.Empty;
        public string BeneficiaryDetail { get; set; } = string.Empty;
        public string? CBCStatus { get; set; }
        public string? ErrorFieldsName { get; set; }
        public string ReceiverBranchCode { get; set; } = string.Empty;
        public string ChequeNumber { get; set; } = string.Empty;
        public string InstrumentNo { get; set; } = string.Empty;
        public string SequenceNumber { get; set; } = string.Empty;
        public string TransactionCode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Returnreasone { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public bool? Callback { get; set; }
        public byte[][]? Signature { get; set; }
    }
}
