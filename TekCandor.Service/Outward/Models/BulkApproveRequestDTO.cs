using System.Collections.Generic;

namespace TekCandor.Service.Outward.Models
{
    public class BulkApproveRequestDTO
    {
        public List<long> ChequeIds { get; set; } = new List<long>();
    }

    public class BulkApproveResponseDTO
    {
        public int TotalRequested { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<long> FailedIds { get; set; } = new List<long>();
    }
}
