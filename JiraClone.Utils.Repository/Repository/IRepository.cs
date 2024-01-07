using JiraClone.Utils.Repository.Helpers;
using JiraClone.Utils.Repository.Transaction;
using System.Data;
using System.Linq.Expressions;

namespace JiraClone.Utils.Repository.Repository
{
    public interface IRepository: IDisposable
    {
        IQueryable<TEntity> All<TEntity>() where TEntity : class;
        ITransaction BeginTransaction();
        ITransaction BeginTransaction(IsolationLevel isolationLevel);
        bool Contain<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<bool> ContainAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        int Count<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        void Create<TEntity>(params TEntity[] entities) where TEntity : class;
        Task CreateAsync<TEntity>(params TEntity[] entities) where TEntity : class;
        int Delete<TEntity, TKey>(TKey id) where TEntity : class;
        int Delete<TEntity, TKey>(TKey[] ids) where TEntity : class;
        int Delete<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        int Delete<TEntity>(TEntity entity) where TEntity : class;
        int Delete<TEntity>(TEntity[] entities) where TEntity : class;
        Task<int> DeleteAsync<TEntity, TKey>(TKey id) where TEntity : class;
        Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids) where TEntity : class;
        Task<int> DeleteAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<int> DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<int> DeleteAsync<TEntity>(TEntity[] entities) where TEntity : class;
        void Dispose();
        IQueryable<TEntity> Filter<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        IQueryable<TEntity> FilterPaged<TEntity>(out long total, PagingParams<TEntity> pagingParams) where TEntity : class;
        IQueryable<TEntity> FilterPaged<TEntity>(PagingParams<TEntity> pagingParams) where TEntity : class;
        TEntity Find<TEntity>(object id) where TEntity : class;
        TEntity Find<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<TEntity> FindAsync<TEntity>(object id) where TEntity : class;
        Task<TEntity> FindAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        TDbContext GetDbContext<TDbContext>() where TDbContext : class;
        void SaveChanges();
        Task SaveChangesAsync();
        int Update<TEntity>(params TEntity[] entities) where TEntity : class;
        Task<int> UpdateAsync<TEntity>(params TEntity[] entities) where TEntity : class;
    }
}