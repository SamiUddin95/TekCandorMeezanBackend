namespace TekCandor.Service.Outward.Models
{
    public class BatchStatisticsDTO
    {
        public int TotalBatchesToday { get; set; }
        public int PendingAuthorization { get; set; }
        public decimal AuthorizedValue { get; set; }
        public int ProcessingExceptions { get; set; }
        public int DraftBatches { get; set; }
        public int AuthorizedBatches { get; set; }
        public int RejectedBatches { get; set; }
    }
}
