using System;

namespace TekCandor.Service.Outward.Models
{
    public class BusinessDateDTO
    {
        public int Id { get; set; }
        public DateTime? BusinessDate { get; set; }
        public bool? IsActive { get; set; }
        public string? StartedBy { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}
