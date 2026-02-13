using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class Permission
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public bool IsDeleted { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get;set; } 




    }
}
