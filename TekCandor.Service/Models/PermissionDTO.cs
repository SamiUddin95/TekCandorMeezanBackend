using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class PermissionDTO
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

}
