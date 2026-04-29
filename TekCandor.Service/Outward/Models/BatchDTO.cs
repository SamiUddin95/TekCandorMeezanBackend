using System;

namespace TekCandor.Service.Outward.Models
{
    public class BatchDTO
    {
        public long Id { get; set; }
        public string BatchId { get; set; } = string.Empty;
        public string? Branch { get; set; }
        public int TotalInstruments { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Draft";
        public int? MaxInstruments { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string? SubmittedBy { get; set; }
        public DateTime? AuthorizedAt { get; set; }
        public string? AuthorizedBy { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectedBy { get; set; }
        public string? RejectionReason { get; set; }
    }
}
