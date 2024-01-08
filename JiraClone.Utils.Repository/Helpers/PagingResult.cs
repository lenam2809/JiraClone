using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Helpers
{
    public class PagingResult<TDto>
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public long TotalRows { get; set; }

        public List<TDto> Data { get; set; }
    }
}
