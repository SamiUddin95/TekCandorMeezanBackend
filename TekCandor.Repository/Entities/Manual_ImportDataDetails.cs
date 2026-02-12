using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class Manual_ImportDataDetails
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public bool Error { get; set; }
        public string ErrorDescription { get; set; }
        public long Manual_ImportDataId { get; set; }
        public Manual_ImportData Manual_ImportData { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
