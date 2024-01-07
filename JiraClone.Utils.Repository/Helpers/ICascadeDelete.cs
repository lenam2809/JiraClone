using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Helpers
{
    public interface ICascadeDelete<TContext>
        where TContext : class
    {
        void OnDelete(TContext context);
    }
}
