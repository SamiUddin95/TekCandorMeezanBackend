using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (TotalCount + PageSize - 1) / PageSize : 0;
    }

    public class UserPagedResult<T> : PagedResult<T>
    {
        public int TotalUsers { get; set; }
        public int TotalHubUser { get; set; }
        public int TotalBranchUser { get; set; }
        public int TotalActiveUser { get; set; }
    }
}
