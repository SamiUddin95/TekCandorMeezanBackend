using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TekCandor.Service.Models
{
    public class HubDTO
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(128)]
        public string? Code { get; set; }
        [MaxLength(256)]
        public string? Name { get; set; }
        public bool? IsNew { get; set; }
        [MaxLength(128)]
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        [MaxLength(128)]
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiesDateTime { get; set; }
        public string? CrAccSameDay { get; set; }
        public string? CrAccNormal { get; set; }
        public string? CrAccIntercity { get; set; }
        public string? CrAccDollar { get; set; }
    }
}
