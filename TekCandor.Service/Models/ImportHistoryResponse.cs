using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ImportHistoryResponse
    {
        public long Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfullRecords { get; set; }
        public int FailureRecords { get; set; }
    }

}
