using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ImportDataDetailResponse
    {
        public long Id { get; set; }
        public long ImportDataId { get; set; }
        public string Data { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Error { get; set; }
        public string? ErrorDescription { get; set; }
    }
}
