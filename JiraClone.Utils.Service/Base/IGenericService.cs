using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Service.Base
{
    public interface IGenericService<TEntity> : IBaseService
        where TEntity : class
    {
        IQueryable<TDto> All<TDto>();

        int Count<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        Task<int> CountAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        TDto Find<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        TDto Find<TDto>(object id);

        Task<TDto> FindAsync<TDto>(object id);

        Task<TDto> FindAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        IQueryable<TDto> Filter<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        IQueryable<TDto> FilterPaged<TDto>(PagingParams<TEntity, TDto> pagingParams);

        IQueryable<TDto> FilterPaged<TDto>(out int total, PagingParams<TEntity, TDto> pagingParams);

        PagingResult<TDto> GetPaged<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class;

        Task<PagingResult<TDto>> GetPagedAsync<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class;

        bool Contain<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        Task<bool> ContainAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        void Create<TDto>(params TDto[] dtos) where TDto : class;

        Task CreateAsync<TDto>(params TDto[] dtos) where TDto : class;

        int Delete<TKey>(TKey id);

        int Delete<TKey>(TKey[] ids);

        int Delete<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        Task<int> DeleteAsync<TKey>(TKey id);

        Task<int> DeleteAsync<TKey>(params TKey[] ids);

        Task<int> DeleteAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        int Update<TDto>(TDto dto, object id) where TDto : class;

        Task<int> UpdateAsync<TDto>(TDto dto, object id) where TDto : class;

        int Update<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>;

        Task<int> UpdateAsync<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>;
    }

    public interface IGenericService<TEntity, TUserKey> : IBaseService<TUserKey>
        where TEntity : class
        where TUserKey : struct
    {
        IQueryable<TDto> All<TDto>();

        int Count<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        Task<int> CountAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        TDto Find<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        TDto Find<TDto>(object id);

        Task<TDto> FindAsync<TDto>(object id);

        Task<TDto> FindAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        IQueryable<TDto> Filter<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        IQueryable<TDto> FilterPaged<TDto>(PagingParams<TEntity, TDto> pagingParams);

        IQueryable<TDto> FilterPaged<TDto>(out int total, PagingParams<TEntity, TDto> pagingParams);

        PagingResult<TDto> GetPaged<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class;

        Task<PagingResult<TDto>> GetPagedAsync<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class;

        bool Contain<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        Task<bool> ContainAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        void Create<TDto>(params TDto[] dtos) where TDto : class;

        Task CreateAsync<TDto>(params TDto[] dtos) where TDto : class;

        int Delete<TKey>(TKey id);

        int Delete<TKey>(TKey[] ids);

        int Delete<TDto>(params Expression<Func<TDto, bool>>[] predicates);

        Task<int> DeleteAsync<TKey>(TKey id);

        Task<int> DeleteAsync<TKey>(TKey[] ids);

        Task<int> DeleteAsync<TDto>(params Expression<Func<TDto, bool>>[] predicates);
        //thêm


        int Update<TDto>(TDto dto, object id) where TDto : class;

        Task<int> UpdateAsync<TDto>(TDto dto, object id) where TDto : class;

        int Update<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>;

        Task<int> UpdateAsync<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>;

        void Create<TDto>(TUserKey userId, params TDto[] dtos) where TDto : class;

        Task CreateAsync<TDto>(TUserKey userId, params TDto[] dtos) where TDto : class;

        int Delete<TKey>(TUserKey userId, TKey id);

        int Delete<TKey>(TUserKey userId, TKey[] ids);

        int Delete<TDto>(TUserKey userId, params Expression<Func<TDto, bool>>[] predicates);

        Task<int> DeleteAsync<TKey>(TUserKey userId, TKey id);

        Task<int> DeleteAsync<TKey>(TUserKey userId, TKey[] ids);

        Task<int> DeleteAsync<TDto>(TUserKey userId, params Expression<Func<TDto, bool>>[] predicates);

        int Update<TDto>(TUserKey userId, object id, TDto dto) where TDto : class;

        Task<int> UpdateAsync<TDto>(TUserKey userId, object id, TDto dto) where TDto : class;

        int Update<TDto, TKey>(TUserKey userId, params TDto[] dtos) where TDto : class, IIdentifier<TKey>;

        Task<int> UpdateAsync<TDto, TKey>(TUserKey userId, params TDto[] dtos) where TDto : class, IIdentifier<TKey>;
    }
}
