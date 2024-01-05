using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Audit
{
    public class AuditLogEntry
    {
        public string ColumnName { get; set; }
        public Type DataType { get; set; }
        public string OriginalValue { get; set; }
        public string CurrentValue { get; set; }
    }
}
