﻿using JiraClone.Utils.Repository.Audit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository
{
    public interface IRepository : IDisposable
    {
        IQueryable<TEntity> All<TEntity>()
            where TEntity : class;

        int Count<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        Task<int> CountAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        TEntity Find<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        TEntity Find<TEntity>(object id)
            where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(object id)
            where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        IQueryable<TEntity> Filter<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        IQueryable<TEntity> FilterPaged<TEntity>(PagingParams<TEntity> pagingParams)
            where TEntity : class;

        IQueryable<TEntity> FilterPaged<TEntity>(out int total, PagingParams<TEntity> pagingParams)
            where TEntity : class;

        PagingResult<TEntity> GetPaged<TEntity>(PagingParams<TEntity> pagingParams)
            where TEntity : class;

        bool Contain<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        Task<bool> ContainAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        void Create<TEntity>(params TEntity[] entities)
            where TEntity : class;

        Task CreateAsync<TEntity>(params TEntity[] entities)
            where TEntity : class;

        int Delete<TEntity, TKey>(TKey id)
            where TEntity : class;

        int Delete<TEntity, TKey>(TKey[] ids)
            where TEntity : class;

        int Delete<TEntity>(TEntity entity)
            where TEntity : class;

        int Delete<TEntity>(TEntity[] entities)
            where TEntity : class;

        int Delete<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TKey id)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity>(TEntity entity)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity>(TEntity[] entity)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        int Update<TEntity>(params TEntity[] entities)
            where TEntity : class;

        Task<int> UpdateAsync<TEntity>(params TEntity[] entities)
            where TEntity : class;

        void SaveChanges();

        Task SaveChangesAsync();

        IEnumerable<AuditLogEntry> GetAuditLogEntries(object entity);

        int ExecuteNonQuery(string sql, params object[] sqlParams);

        TResult ExecuteReader<TResult>(string sql, params object[] sqlParams);

        TContext GetDbContext<TContext>() where TContext : class;

        ITransaction BeginTransaction();

        ITransaction BeginTransaction(IsolationLevel isolationLevel);
    }

    public interface IRepository<TUserKey> : IRepository, IDisposable where TUserKey : struct
    {
        void SetBatchInsert(bool inBatchInsert);
        void Commit();
        void Create<TEntity>(TUserKey userId, params TEntity[] entities)
            where TEntity : class;

        Task CreateAsync<TEntity>(TUserKey userId, params TEntity[] entities)
            where TEntity : class;

        int Delete<TEntity, TKey>(TUserKey userId, TKey id)
            where TEntity : class;

        int Delete<TEntity, TKey>(TUserKey userId, TKey[] ids)
            where TEntity : class;

        int Delete<TEntity>(TUserKey userId, TEntity entity)
            where TEntity : class;

        int Delete<TEntity>(TUserKey userId, TEntity[] entities)
            where TEntity : class;

        int Delete<TEntity>(TUserKey userId, params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TUserKey userId, TKey id)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TUserKey userId, TKey[] ids)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity>(TUserKey userId, TEntity entity)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity>(TUserKey userId, TEntity[] entity)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity>(TUserKey userId, params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class;

        int Update<TEntity>(TUserKey userId, params TEntity[] entities)
            where TEntity : class;

        Task<int> UpdateAsync<TEntity>(TUserKey userId, params TEntity[] entities)
            where TEntity : class;
    }

}
