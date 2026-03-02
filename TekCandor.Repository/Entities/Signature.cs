using System;

namespace TekCandor.Repository.Entities
{
    public class Signature
    {
        public long Id { get; set; }
        public string? AccountNumber { get; set; }
        public byte[]? Sign { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
