using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class AssignPermissionsDTO
    {
        public long GroupId { get; set; }
        public List<long> PermissionIds { get; set; } = new();
    }
}
