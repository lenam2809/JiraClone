using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository
{
    public class PagingResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<T> Items { get; set; }

        public PagingResult()
        {
            Items = new List<T>();
        }
    }
}
