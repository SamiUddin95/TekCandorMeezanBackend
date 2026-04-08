using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Models;

namespace TekCandor.Service.Models
{
    public class GroupDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }
        public int Version { get; set; }
        public bool IsNew { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<PermissionDetailDTO>? Permissions { get; set; }
        public List<UserDetailDTO>? Users { get; set; }
    }
}
