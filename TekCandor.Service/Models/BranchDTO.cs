using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Service.Models
{
    public class BranchDTO
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? NIFTBranchCode { get; set; }
        public string? Name { get; set; }
        public long HubId { get; set; }
       
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public string? Email3 { get; set; }

    }
}
