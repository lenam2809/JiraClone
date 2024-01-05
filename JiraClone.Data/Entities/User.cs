using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data.Entities
{
    public partial class User : IdentityUser<int>
    {
        public User()
        {
            
        }

        [StringLength(250)]
        public string FullName { get; set; }
        public bool IsAdministrator { get; set; }
        public int Status { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUserId { get; set; }
        public virtual User DeletedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserDetail> UserDetails { get; set; }
    }
}
