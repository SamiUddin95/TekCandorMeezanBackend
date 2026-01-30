using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class Branch
    {
        public required Guid Id { get; set; }
        public string? NIFT { get; set; }
        public string? Code { get; set; }
        public string? NIFTBranchCode { get; set; }
        public  string? Name { get; set; }
        public  Guid? HubId { get; set; }
        public  int Version { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }

        public string? CreatedUser { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public string? Email3 { get; set; }
    }
}
