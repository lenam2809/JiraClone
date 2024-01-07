using AutoMapper;
using JiraClone.Utils.BaseService;
using JiraClone.Utils.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Helpers
{
    public class JiraCloneBaseService : BaseService
    {
        public JiraCloneBaseService(Repository repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
