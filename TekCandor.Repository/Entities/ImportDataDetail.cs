using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class ImportDataDetail
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public bool Error { get; set; }
        public string ErrorDescription { get; set; }
        public long ImportDataId {  get; set; }
        public ImportData ImportData { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
