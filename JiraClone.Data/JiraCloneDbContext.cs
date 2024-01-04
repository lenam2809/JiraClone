using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
