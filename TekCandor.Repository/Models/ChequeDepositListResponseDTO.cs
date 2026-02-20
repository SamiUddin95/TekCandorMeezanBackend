using System;

namespace TekCandor.Repository.Models
{
    public class ChequeDepositListResponseDTO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string? SenderBankCode { get; set; }
        public string? ReceiverBranchCode { get; set; }
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? TransactionCode { get; set; }
        public string? Status { get; set; }
        public string? AccountBalance { get; set; }
        public string? AccountStatus { get; set; }
        public string? Currency { get; set; }
        public string? HubCode { get; set; }
        public string? CycleCode { get; set; }
        public string? InstrumentNo { get; set; }
        public string? BranchRemarks { get; set; }
        public bool Error { get; set; }
        public string? Callbacksend { get; set; }
        public bool? Export { get; set; }
    }
}
