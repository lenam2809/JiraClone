using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Audit
{
    /// <summary>
    /// The entity that is used to store CRUD actions on other entity
    /// </summary>
    /// <typeparam name="TUser">The type of user entity that perform the action</typeparam>
    /// <typeparam name="TUserKey">The type of user's key</typeparam>
    public interface IAuditLog<TUser, TUserKey>
        where TUser : IIdentifier<TUserKey>
        where TUserKey : struct
    {
        string Origin { get; set; }

        string EntityName { get; set; }
        string FieldName { get; set; }
        int LogType { get; set; }

        string GroupName { get; set; }

        string EntityValue { get; set; }

        string EntityText { get; set; }

        string FromValue { get; set; }
        string ToValue { get; set; }

        DateTime LogDate { get; set; }

        TUserKey LogByUserId { get; set; }
        TUser LogByUser { get; set; }
    }

    public class LogType
    {
        [Description("Thêm mới")]
        public const int Create = 1;
        [Description("Sửa")]
        public const int Update = 2;
        [Description("Xóa")]
        public const int Delete = 4;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class AuditLogAttribute : Attribute
    {
        public string TextProperty { get; set; }
        public Type ReferenceType { get; set; }
        public string ReferenceProperty { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }

        public AuditLogAttribute()
        {

        }
    }
}
