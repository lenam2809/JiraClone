using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Audit
{
    /// <summary>
    /// Entity need to be extended from this interface to set the updated date automatically
    /// </summary>
    public interface IUpdateInfo
    {
        DateTime? LastUpdatedDate { get; set; }
    }

    /// <summary>
    /// Entity need to be extended from this interface to set the user that perform the update automatically
    /// </summary>
    /// <typeparam name="TUser">The user's type that perform the update</typeparam>
    /// <typeparam name="TUserKey">The user's key type</typeparam>
    public interface IUpdateInfo<TUser, TUserKey> : IUpdateInfo
        where TUser : IIdentifier<TUserKey>
        where TUserKey : struct
    {
        TUserKey? LastUpdatedUserId { get; set; }
        TUser LastUpdatedUser { get; set; }
    }
}
