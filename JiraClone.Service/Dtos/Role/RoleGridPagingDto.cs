using JiraClone.Service.Helpers;
using JiraClone.Utils.BaseService;
using JiraClone.Utils.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Dtos.Role
{
    public class RoleGridPagingDto : PagingParams<RoleGridDto>
    {
        public string FilterText { get; set; }
        public override List<Expression<Func<RoleGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.Name.Contains(FilterText.Trim()) || x.Description.Contains(FilterText.Trim()));
            }
            return predicates;
        }
    }
}
