using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class ClearingStatuses
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public string? Value { get; set; }
        public bool Version { get; set; }
        public string? Name { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
