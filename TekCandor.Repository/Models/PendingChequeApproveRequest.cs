namespace TekCandor.Repository.Models
{
    public class PendingChequeApproveRequest
    {
        public long Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? ChequeNumber { get; set; }
    }
}
