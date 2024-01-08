using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data.Enums
{
    public enum ActionLogs
    {
        [Description("Tạo mới")]
        Add = 1,
        [Description("Lưu nháp")]
        Draft = 2,
        [Description("Đồng bộ")]
        Sync = 3,
        [Description("Import")]
        Import = 4,
        [Description("Sửa")]
        Edit = 5,
        [Description("Xóa")]
        Delete = 6,
        [Description("Export")]
        Export = 7,
        [Description("Khóa")]
        Lock = 8,
        [Description("Mở khóa")]
        Unlock = 9,
        [Description("Gửi duyệt")]
        Pending = 10,
        [Description("Phê duyệt")]
        Approved = 11,
        [Description("Từ chối")]
        Deny = 12,
        [Description("Ẩn")]
        Hide,
        [Description("Hiện")]
        Show,
        [Description("Đăng nhập")]
        Login,
        [Description("Truy cập trang web")]
        Access,
    }
}
