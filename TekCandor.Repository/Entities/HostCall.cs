using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class HostCall
    {
        public long Id { get; set; }
        public long ChequeDeposit_Id { get; set; }
        public DateTime RequsetDate { get; set; }
        public DateTime ResponseDate {  get; set; }
        public string URL {  get; set; }
        public string RequestMsg { get; set; }
        public string ResponseMsg { get; set; }
        public string ResponseCode { get; set; }
        public bool IsApproved { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
