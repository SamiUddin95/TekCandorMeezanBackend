namespace TekCandor.Service.Outward.Models
{
    public class FundRealizationDTO
    {
        public string? ReceiverBranchCode { get; set; }
        public string? BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public int ChequeCount { get; set; }
    }
}
