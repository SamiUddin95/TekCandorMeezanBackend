using System.Collections.Generic;

namespace TekCandor.Repository.Models
{
    public class PendingApproveSelectedRequest
    {
        public List<long> SelectedIds { get; set; } = new List<long>();
    }
}
