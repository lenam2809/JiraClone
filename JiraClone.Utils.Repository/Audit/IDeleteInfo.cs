using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Audit
{
    /// <summary>
    /// Entity need to be extended from this interface to set the deleted date to mark the record's deletion, instead of deleting the record from database
    /// </summary>
    public interface IDeleteInfo
    {
        DateTime? DeletedDate { get; set; }
    }

    /// <summary>
    /// Entity need to be extended from this interface to set the user that delete the record automatically. The record should be marked as deleted only, not actual deleted from database
    /// </summary>
    /// <typeparam name="TUser">The user's type that perform the deletion</typeparam>
    /// <typeparam name="TUserKey">The user's key type</typeparam>
    public interface IDeleteInfo<TUser, TUserKey> : IDeleteInfo
        where TUser : IIdentifier<TUserKey>
        where TUserKey : struct
    {
        TUserKey? DeletedUserId { get; set; }
        TUser DeletedUser { get; set; }
    }
}
