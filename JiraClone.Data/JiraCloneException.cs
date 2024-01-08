using JiraClone.Utils.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data
{
    public class JiraCloneException : ApplicationException
    {
        public JiraCloneException(JiraCloneExceptionCode code, params object[] args) : base(string.Format(code.GetEnumDescription(), args)) { }

        public JiraCloneException(string message) : base(message) { }
    }

    public enum JiraCloneExceptionCode
    {
        [Description("Không thể xóa {0} do có dữ liệu liên quan.")]
        DeleteRecordWithRelatedData = 1
    }
}
