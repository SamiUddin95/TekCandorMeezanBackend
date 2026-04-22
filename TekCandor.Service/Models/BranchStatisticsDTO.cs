namespace TekCandor.Service.Models
{
    public class BranchStatisticsDTO
    {
        public string BranchCode { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public int TotalInstrumentCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
