namespace TekCandor.Repository.Models
{
    public class PendingApproveSelectedResponse
    {
        public int SuccessCount { get; set; }
        public string ChequeNumbers { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
