﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data.Entities
{
    public partial class GroupRolePermission
    {
        public GroupRolePermission()
        {
        }
        public int Id { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("GroupRolePermissions")]
        public virtual Group Group { get; set; }

        //1 - Có, 0 - Không
        [Required]
        public int Value { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("GroupRolePermissions")]
        public virtual Role Role { get; set; }
        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        [InverseProperty("GroupRolePermissions")]
        public virtual Permission Permission { get; set; }
    }
}
