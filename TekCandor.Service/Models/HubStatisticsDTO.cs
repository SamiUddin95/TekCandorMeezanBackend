namespace TekCandor.Service.Models
{
    public class HubStatisticsDTO
    {
        public string HubCode { get; set; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
        public int TotalInstrumentCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
