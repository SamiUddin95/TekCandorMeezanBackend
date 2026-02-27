using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ReturnReasonDTO
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? AlphaReturnCodes { get; set; }
        public int NumericReturnCodes { get; set; }
        public string? DescriptionWithReturnCodes { get; set; }
        public bool DefaultCallBack { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }


    }
}
