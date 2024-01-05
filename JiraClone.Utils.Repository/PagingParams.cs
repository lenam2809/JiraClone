using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository
{
    public class PagingParams<TEntity>
        where TEntity : class
    {
        public static int DefaultPageSize = 10;
        public string SortExpression { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int StartingIndex { get { return PageSize * Page; } }
        public List<Expression<Func<TEntity, bool>>> Predicates { get; set; }

        public PagingParams(Expression<Func<TEntity, bool>> predicate = null)
        {
            SortExpression = "Id desc";
            PageSize = DefaultPageSize;
            Page = 0;
            Predicates = new List<Expression<Func<TEntity, bool>>>();
            if (predicate != null)
            {
                Predicates.Add(predicate);
            }
        }
    }
}
