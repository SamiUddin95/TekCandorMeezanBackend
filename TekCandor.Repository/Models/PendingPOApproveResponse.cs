namespace TekCandor.Repository.Models
{
    public class PendingPOApproveResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
