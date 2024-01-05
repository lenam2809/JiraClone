using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraClone.Data.Entities
{
    public class GroupUser
    {
        public GroupUser()
        {
        }
        public int Id { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("GroupUsers")]
        public virtual Group Group { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("GroupUsers")]
        public virtual User User { get; set; }
    }
}
