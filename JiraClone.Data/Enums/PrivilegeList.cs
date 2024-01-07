using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data.Enums
{
    public enum PrivilegeList
    {
        [Description("Thêm mới")]
        Add = 1,
        [Description("Sửa")]
        Edit = 2,
        [Description("Xóa")]
        Delete = 3,
        [Description("Phê duyệt")]
        Approved = 4,
        [Description("Quyền chức năng")]
        Permission = 5,
    }
}
