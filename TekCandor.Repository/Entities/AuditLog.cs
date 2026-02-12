using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string Action { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedOn { get; set; }
    }
}
