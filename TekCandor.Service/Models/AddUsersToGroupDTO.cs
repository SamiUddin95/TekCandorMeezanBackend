using System.Collections.Generic;

namespace TekCandor.Service.Models
{
    public class AddUsersToGroupDTO
    {
        public long SecurityGroupId { get; set; }
        public List<long> SelectedIds { get; set; } = new List<long>();
    }
}
