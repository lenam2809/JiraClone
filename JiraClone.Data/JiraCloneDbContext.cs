using JiraClone.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiraClone.Data
{
    public partial class JiraCloneDbContext : DbContext 
    {
        public JiraCloneDbContext()
        {
        }

        public JiraCloneDbContext(DbContextOptions<JiraCloneDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IdentityClient> IdentityClients { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDetail> UserDetail { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
    }
}
