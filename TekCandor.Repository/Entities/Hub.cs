using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class Hub
    {
        public long Id { get; set; }
        public int Version { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(128)]
        public string? Code { get; set; }
        [MaxLength(256)]
        public string? Name { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? CrAccSameDay { get; set; }
        public string? CrAccNormal { get; set; }
        public string? CrAccIntercity { get; set; }
        public string? CrAccDollar { get; set; }
    }
}
