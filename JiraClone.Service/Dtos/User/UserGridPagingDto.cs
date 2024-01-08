using JiraClone.Service.Helpers;
using JiraClone.Utils.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Dtos.User
{
    public class UserGridPagingDto : PagingParams<UserGridDto>
    {
        public string Username { get; set; }
        public string FilterText { get; set; }
        public int GroupId { get; set; }

        public override List<Expression<Func<UserGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.Username.Contains(FilterText.Trim()) || x.FullName.Contains(FilterText.Trim()));
            }
            if (!string.IsNullOrEmpty(Username))
            {
                predicates.Add(x => x.Username.Contains(Username));
            }
            predicates.Add(x => x.DeletedUserId == null);
            return predicates;
        }

        public List<Expression<Func<Data.Entities.User, bool>>> GetTraversalPredicates()
        {
            var predicates = new List<Expression<Func<Data.Entities.User, bool>>>();

            return predicates;
        }

    }
}
