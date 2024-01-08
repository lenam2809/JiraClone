using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data.Enums
{
    public enum StatusLogs
    {
        [Description("Thành công")]
        Success = 1,
        [Description("Thất bại")]
        Error = 2,
    }
}
