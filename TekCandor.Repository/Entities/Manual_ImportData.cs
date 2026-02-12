using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class Manual_ImportData
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfullRecords { get; set; }
        public int FailureRecords { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
