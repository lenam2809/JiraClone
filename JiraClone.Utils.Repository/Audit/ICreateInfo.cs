using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Audit
{
    /// <summary>
    /// Entity need to be extended from this interface to set the created date automatically
    /// </summary>
    public interface ICreateInfo
    {
        DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Entity need to be extended from this interface to set the user that perform the creation automatically
    /// </summary>
    /// <typeparam name="TUser">The user's type that perform the creation</typeparam>
    /// <typeparam name="TUserKey">The user's key type</typeparam>
    public interface ICreateInfo<TUser, TUserKey> : ICreateInfo
        where TUser : IIdentifier<TUserKey>
        where TUserKey : struct
    {
        TUserKey CreatedUserId { get; set; }
        TUser CreatedUser { get; set; }
    }
}
