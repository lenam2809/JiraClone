using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Audit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Service.Base
{
    public interface IBaseService : IDisposable
    {
        IQueryable<TDto> All<TEntity, TDto>()
            where TEntity : class;

        int Count<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        Task<int> CountAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        TDto Find<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        TDto Find<TEntity, TDto>(object id)
            where TEntity : class;

        Task<TDto> FindAsync<TEntity, TDto>(object id)
            where TEntity : class;

        Task<TDto> FindAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        IQueryable<TDto> Filter<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        IQueryable<TDto> FilterPaged<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class;

        IQueryable<TDto> FilterPaged<TEntity, TDto>(out int total, PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class;

        PagingResult<TDto> GetPaged<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
            where TDto : class;

        Task<PagingResult<TDto>> GetPagedAsync<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
            where TDto : class;

        bool Contain<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        Task<bool> ContainAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        void Create<TEntity, TDto>(params TDto[] dtos)
            where TEntity : class
            where TDto : class;

        Task CreateAsync<TEntity, TDto>(params TDto[] dtos)
            where TEntity : class
            where TDto : class;

        int Delete<TEntity, TKey>(TKey id)
            where TEntity : class;

        int Delete<TEntity, TKey>(TKey[] ids)
            where TEntity : class;

        int Delete<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TKey id)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        int Update<TEntity, TDto>(TDto dto, object id)
            where TEntity : class
            where TDto : class;

        Task<int> UpdateAsync<TEntity, TDto>(TDto dto, object id)
            where TEntity : class
            where TDto : class;


        int Update<TEntity, TDto, TKey>(params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>;

        Task<int> UpdateAsync<TEntity, TDto, TKey>(params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>;

        int ExecuteNonQuery(string sql, params object[] sqlParams);

        TResult ExecuteReader<TResult>(string sql, params object[] sqlParams);

        TContext GetDbContext<TContext>() where TContext : class;

        ITransaction BeginTransaction();

        ITransaction BeginTransaction(IsolationLevel isolationLevel);
    }

    public interface IBaseService<TUserKey> : IBaseService, IDisposable where TUserKey : struct
    {
        void SetBatchInsert(bool inBatchInsert);
        void Commit();
        void Create<TEntity, TDto>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class;

        Task CreateAsync<TEntity, TDto>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class;

        int Delete<TEntity, TKey>(TUserKey userId, TKey id)
            where TEntity : class;

        int Delete<TEntity, TKey>(TUserKey userId, TKey[] ids)
            where TEntity : class;

        int Delete<TEntity, TDto>(TUserKey userId, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TUserKey userId, TKey id)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TKey>(TUserKey userId, TKey[] ids)
            where TEntity : class;

        Task<int> DeleteAsync<TEntity, TDto>(TUserKey userId, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class;

        int Update<TEntity, TDto>(TUserKey userId, object id, TDto dto)
            where TEntity : class
            where TDto : class;

        Task<int> UpdateAsync<TEntity, TDto>(TUserKey userId, object id, TDto dto)
            where TEntity : class
            where TDto : class;

        int Update<TEntity, TDto, TKey>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>;

        Task<int> UpdateAsync<TEntity, TDto, TKey>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>;
    }

}
