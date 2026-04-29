namespace TekCandor.Service.Outward.Models
{
    public class RejectBatchDTO
    {
        public string BatchId { get; set; } = string.Empty;
        public string RejectionReason { get; set; } = string.Empty;
    }
}
