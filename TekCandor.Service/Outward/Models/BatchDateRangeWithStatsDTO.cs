namespace TekCandor.Service.Outward.Models
{
    public class BatchDateRangeWithStatsDTO
    {
        public List<BatchDTO> Batches { get; set; } = new List<BatchDTO>();
        public BatchStatisticsDTO Statistics { get; set; } = new BatchStatisticsDTO();
    }
}
