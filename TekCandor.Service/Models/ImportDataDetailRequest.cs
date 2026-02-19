using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Models
{
    public class ImportDataDetailRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortColumn { get; set; } = "Date";
        public string? SortDirection { get; set; } = "DESC"; // ASC | DESC
    }
}
