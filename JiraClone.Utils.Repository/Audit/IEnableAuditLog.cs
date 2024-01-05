using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository.Audit
{
    /// <summary>
    /// Entity needs to be extended from this interface to log CRUD actions on the entity to this table
    /// </summary>
    /// <typeparam name="TUser">The type of user entity that perform the action</typeparam>
    /// <typeparam name="TUserKey">The type of user's key</typeparam>
    public interface IEnableAuditLog<TUserKey>
        where TUserKey : struct
    {
        void OnCreateLog<TEntity>(IRepository<TUserKey> repo, TEntity entity, TUserKey userKey)
            where TEntity : class;
        void OnUpdateLog<TEntity>(IRepository<TUserKey> repo, TEntity entity, TUserKey userKey)
            where TEntity : class;
        void OnDeleteLog<TEntity>(IRepository<TUserKey> repo, TEntity entity, TUserKey userKey)
            where TEntity : class;
    }
}
