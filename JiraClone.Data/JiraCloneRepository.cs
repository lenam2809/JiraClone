using JiraClone.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data
{
    public class JiraCloneRepository : Repository<JiraCloneDbContext, User, int>
    {
        public JiraCloneRepository(JiraCloneDbContext dbContext, IUserIdentity<int> currentUser, ILogger<JiraCloneRepository> logger) : base(dbContext, currentUser, logger)
        {

        }
    }
}
