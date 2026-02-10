using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class GetBranchDTO
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? NIFTBranchCode { get; set; }
        public string? Name { get; set; }
        public long HubId { get; set; }
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public string? Email3 { get; set; }
    }
}
