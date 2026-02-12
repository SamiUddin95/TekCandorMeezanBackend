using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? LoginName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? BranchorHub { get; set; }
        [MaxLength(255)]
        public string? HubIds { get; set; }
       
        [MaxLength(255)]
        public string? BranchIds { get; set; }
       
        public long? GroupId { get; set; }
        public Group Group { get; set; }
        public bool IsDeleted { get; set; }

        public string? PhoneNo { get; set; }
        public string? PasswordLastChanged { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UserLimit { get; set; }




    }
}
