using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class RevokedToken
    {
        public long Id { get; set; }
        public string Jti { get; set; } = string.Empty;
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}


