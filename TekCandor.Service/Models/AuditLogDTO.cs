using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TekCandor.Repository.Models.dto;

namespace TekCandor.Service.Models
{
    public class AuditLogDTO
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
        public string User { get; set; }
        public DateTime Time { get; set; }

        [JsonIgnore]
        public string OldValues_Internal { get; set; }
        [JsonIgnore]
        public string NewValues_Internal { get; set; }
        public List<FieldChangeDTO> Details { get; set; } = new();

    }
}

