using System.Collections.Generic;

namespace TekCandor.Service.Outward.Models
{
    public class SupervisorListGroupedResult
    {
        public List<BatchGroupedChequeDTO> Batches { get; set; } = new List<BatchGroupedChequeDTO>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (TotalCount + PageSize - 1) / PageSize : 0;
    }
}
