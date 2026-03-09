using System.Collections.Generic;

namespace TekCandor.Repository.Models
{
    public class PendingToInprocessRequest
    {
        public List<long> SelectedIds { get; set; } = new List<long>();
    }
}
