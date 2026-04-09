using System;

namespace TekCandor.Repository.Models
{
    public class UserDetailDTO
    {
        public long Id { get; set; }
        public string? LoginName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
