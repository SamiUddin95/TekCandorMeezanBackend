using System.Collections.Generic;

namespace TekCandor.Service.Outward.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (TotalCount + PageSize - 1) / PageSize : 0;
    }
}
