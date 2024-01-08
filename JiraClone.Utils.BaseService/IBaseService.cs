using JiraClone.Utils.Repository.Helpers;
using JiraClone.Utils.Repository.Transaction;
using System.Data;
using System.Linq.Expressions;

namespace JiraClone.Utils.BaseService
{
    public interface IBaseService
    {
        IQueryable<TDto> All<TEntity, TDto>() where TEntity : class;
        IQueryable<TDto> All<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping) where TEntity : class;
        ITransaction BeginTransaction();
        ITransaction BeginTransaction(IsolationLevel isolationLevel);
        bool Contain<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<bool> ContainAsync<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        int Count<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<int> CountAsync<TEntity, TDto>(Expression<Func<TEntity, bool>>[] predicate) where TEntity : class;
        void Create<TEntity, TDto>(Func<TDto, TEntity> mapping, params TDto[] dtos)
            where TEntity : class
            where TDto : class;
        void Create<TEntity, TDto>(params TDto[] dtos)
            where TEntity : class
            where TDto : class;
        Task CreateAsync<TEntity, TDto>(Func<TDto, TEntity> mapping, params TDto[] dtos)
            where TEntity : class
            where TDto : class;
        Task CreateAsync<TEntity, TDto>(params TDto[] dtos)
            where TEntity : class
            where TDto : class;
        int Delete<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        int Delete<TEntity, TKey>(TKey id) where TEntity : class;
        int Delete<TEntity, TKey>(TKey[] ids) where TEntity : class;
        Task<int> DeleteAsync<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        Task<int> DeleteAsync<TEntity, TKey>(TKey id) where TEntity : class;
        Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids) where TEntity : class;
        void Dispose();
        IQueryable<TDto> Filter<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class;
        PagingResult<TDto> FilterPaged<TEntity, TDto>(PagingParams<TEntity> pagingParams) where TEntity : class;
        TDto Find<TEntity, TDto>(Expression<Func<TDto, bool>>[] predicates) where TEntity : class;
        TDto Find<TEntity, TDto>(object id) where TEntity : class;
        Task<TDto> FindAsync<TEntity, TDto>(object id) where TEntity : class;
        Task<TDto> FindAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class;
        TContext GetDbContext<TContext>() where TContext : class;
        int Update<TEntity, TDto>(Action<TDto, TEntity> mapping, object id, TDto dto)
            where TEntity : class
            where TDto : class;
        int Update<TEntity, TDto>(object id, TDto dto)
            where TEntity : class
            where TDto : class;
        Task<int> UpdateAsync<TEntity, TDto>(Action<TDto, TEntity> mapping, object id, TDto dto)
            where TEntity : class
            where TDto : class;
        Task<int> UpdateAsync<TEntity, TDto>(object id, TDto dto)
            where TEntity : class
            where TDto : class;
    }
}