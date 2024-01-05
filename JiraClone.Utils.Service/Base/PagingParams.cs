using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Service.Base
{
    public class PagingParams<TEntity, TDto>
        where TEntity : class
    {
        public static int DefaultPageSize = 10;

        public string SortExpression { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int StartingIndex { get { return PageSize * Page; } }
        public List<Expression<Func<TDto, bool>>> Predicates { get; set; }

        public PagingParams(Expression<Func<TDto, bool>> predicate = null)
        {
            SortExpression = "Id desc";
            PageSize = DefaultPageSize;
            Page = 0;
            Predicates = new List<Expression<Func<TDto, bool>>>();
            if (predicate != null)
            {
                Predicates.Add(predicate);
            }
        }
    }
}
