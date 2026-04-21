using System;

namespace TekCandor.Repository.Entities.Outward
{
    public class BusinessDate
    {
        public int Id { get; set; }
        public DateTime? BusinessDate1 { get; set; }
        public bool? IsActive { get; set; }
        public string? StartedBy { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}
