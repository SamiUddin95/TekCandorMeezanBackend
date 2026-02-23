using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class Currency
    {
        public long Id { get; set; }   

        public string? CurrencyCode { get; set; }

        public decimal Rate { get; set; }   

        public string? DisplayLocale { get; set; }

        public string? CustomFormatting { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public string? Description { get; set; }

        public int Version { get; set; }

        public string? Name { get; set; }

        public bool IsNew { get; set; }

        public bool IsDeleted { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
