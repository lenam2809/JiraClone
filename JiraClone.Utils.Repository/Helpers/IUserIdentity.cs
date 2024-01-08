using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Helpers
{
    public interface IUserIdentity<TUserKey> where TUserKey : struct
    {
        TUserKey UserId { get; }

        string Username { get; }

        List<string> Privileges { get; }

        bool IsAdministrator { get; }

        string UnitId { get; }

        string UnitCode { get; }

        int TaiKhoanID { get; }

        string FullName { get; }
    }
}
