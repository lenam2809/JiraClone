using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Helpers
{
    public class PagingParams<TDto>
    {
        public static int DefaultPageSize = 10;

        //public string SortExpression => (string.IsNullOrEmpty(SortBy) ? "ID" : SortBy) + " " + (SortDesc ? "desc" : "asc");
        public Expression<Func<TDto, object>> SortExpression { get; set; } // Change object to your actual sort type
        public SortDirection SortDirection { get; set; }

        public string SortBy { get; set; }

        public int ItemsPerPage { get; set; }

        public int Page { get; set; }

        public int StartingIndex => ItemsPerPage * (Page - 1);

        public int Start { get; set; }

        public PagingParams()
        {
            SortBy = "Id";
            ItemsPerPage = DefaultPageSize;
            SortDirection = SortDirection.Descending;
            Page = 0;
        }

        public virtual List<Expression<Func<TDto, bool>>> GetPredicates()
        {
            return new List<Expression<Func<TDto, bool>>>();
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
