using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Entities;

namespace TekCandor.Service.Models
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? LoginName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? BranchorHub { get; set; }
  
        public string? HubIds { get; set; }
        public string? BranchIds { get; set; }
        public long? GroupId { get; set; }
        public string? PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UserLimit { get; set; }

        public List<string> Permissions { get; set; } = new();



    }
}
