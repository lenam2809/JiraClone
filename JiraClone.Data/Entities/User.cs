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
        public string FullName { get; init; }
        public bool IsAdministrator { get; init; }
        public int Status { get; init; }
        public DateTime? DeletedDate { get; init; }
        public int? DeletedUserId { get; init; }
        public virtual User DeletedUser { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime ModifiedDate { get; init; }
        [InverseProperty("User")]
        public virtual ICollection<GroupUser> GroupUsers { get; init; }
        [InverseProperty("User")]
        public virtual ICollection<UserDetail> UserDetails { get; init; }
    }
}
