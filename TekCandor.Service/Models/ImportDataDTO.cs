using System;

namespace TekCandor.Service.Models
{
    public class ImportDataDTO
    {
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public long TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailureRecords { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
    }
}
