using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class ReturnReason
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? AlphaReturnCodes { get; set; }
        public string NumericReturnCodes { get; set; }
        public string? DescriptionWithReturnCodes { get; set; }
        public bool DefaultCallBack { get; set; }
        public int Version { get; set; }
        public string? Name { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDateTime { get; set; }


    }
}
