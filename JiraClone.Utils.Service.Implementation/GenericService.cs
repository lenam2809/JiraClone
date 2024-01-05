using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Audit;
using JiraClone.Utils.Service.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Service.Implementation
{
    public class GenericService<TEntity> : BaseService, IGenericService<TEntity>
        where TEntity : class
    {
        public GenericService(IRepository repository)
            : base(repository) { }

        #region IGenericService<TEntity,TDto,TKey> Members

        public virtual IQueryable<TDto> All<TDto>()
        {
            return base.All<TEntity, TDto>();
        }

        public virtual int Count<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Count<TEntity, TDto>(predicates);
        }

        public virtual async Task<int> CountAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.CountAsync<TEntity, TDto>(predicates);
        }

        public virtual TDto Find<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Find<TEntity, TDto>(predicates);
        }

        public virtual TDto Find<TDto>(object id)
        {
            return base.Find<TEntity, TDto>(id);
        }

        public virtual async Task<TDto> FindAsync<TDto>(object id)
        {
            return await base.FindAsync<TEntity, TDto>(id);
        }

        public virtual async Task<TDto> FindAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.FindAsync<TEntity, TDto>(predicates);
        }

        public virtual IQueryable<TDto> Filter<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Filter<TEntity, TDto>(predicates);
        }

        public virtual IQueryable<TDto> FilterPaged<TDto>(PagingParams<TEntity, TDto> pagingParams)
        {
            return base.FilterPaged<TEntity, TDto>(pagingParams);
        }

        public virtual IQueryable<TDto> FilterPaged<TDto>(out int total, PagingParams<TEntity, TDto> pagingParams)
        {
            return base.FilterPaged<TEntity, TDto>(out total, pagingParams);
        }

        public virtual PagingResult<TDto> GetPaged<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class
        {
            return base.GetPaged<TEntity, TDto>(pagingParams);
        }

        public virtual async Task<PagingResult<TDto>> GetPagedAsync<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class
        {
            return await base.GetPagedAsync<TEntity, TDto>(pagingParams);
        }

        public virtual bool Contain<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Contain<TEntity, TDto>(predicates);
        }

        public virtual async Task<bool> ContainAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.ContainAsync<TEntity, TDto>(predicates);
        }

        public virtual void Create<TDto>(params TDto[] dtos) where TDto : class
        {
            base.Create<TEntity, TDto>(dtos);
        }

        public virtual async Task CreateAsync<TDto>(params TDto[] dtos) where TDto : class
        {
            await base.CreateAsync<TEntity, TDto>(dtos);
        }

        public virtual int Delete<TKey>(TKey id)
        {
            return base.Delete<TEntity, TKey>(id);
        }

        public virtual int Delete<TKey>(TKey[] ids)
        {
            return base.Delete<TEntity, TKey>(ids);
        }

        public virtual int Delete<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Delete<TEntity, TDto>(predicates);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TKey id)
        {
            return await base.DeleteAsync<TEntity, TKey>(id);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TKey[] ids)
        {
            return await base.DeleteAsync<TEntity, TKey>(ids);
        }

        public virtual async Task<int> DeleteAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.DeleteAsync<TEntity, TDto>(predicates);
        }

        public virtual int Update<TDto>(TDto dto, object id) where TDto : class
        {
            return base.Update<TEntity, TDto>(dto, id);
        }

        public virtual async Task<int> UpdateAsync<TDto>(TDto dto, object id) where TDto : class
        {
            return await base.UpdateAsync<TEntity, TDto>(dto, id);
        }

        public virtual int Update<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>
        {
            return base.Update<TEntity, TDto, TKey>(dtos);
        }

        public virtual async Task<int> UpdateAsync<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>
        {
            return await base.UpdateAsync<TEntity, TDto, TKey>(dtos);
        }


        #endregion
    }


    public class GenericService<TEntity, TUserKey> : BaseService<TUserKey>, IGenericService<TEntity, TUserKey>
        where TEntity : class
        where TUserKey : struct
    {
        public GenericService(IRepository<TUserKey> repository)
            : base(repository) { }

        #region IGenericService<TEntity,TDto,TKey,TUserKey> Members

        public virtual IQueryable<TDto> All<TDto>()
        {
            return base.All<TEntity, TDto>();
        }

        public virtual int Count<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Count<TEntity, TDto>(predicates);
        }

        public virtual async Task<int> CountAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.CountAsync<TEntity, TDto>(predicates);
        }

        public virtual TDto Find<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Find<TEntity, TDto>(predicates);
        }

        public virtual TDto Find<TDto>(object id)
        {
            return base.Find<TEntity, TDto>(id);
        }

        public virtual async Task<TDto> FindAsync<TDto>(object id)
        {
            return await base.FindAsync<TEntity, TDto>(id);
        }

        public virtual async Task<TDto> FindAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.FindAsync<TEntity, TDto>(predicates);
        }

        public virtual IQueryable<TDto> Filter<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Filter<TEntity, TDto>(predicates);
        }

        public virtual IQueryable<TDto> FilterPaged<TDto>(PagingParams<TEntity, TDto> pagingParams)
        {
            return base.FilterPaged<TEntity, TDto>(pagingParams);
        }

        public virtual IQueryable<TDto> FilterPaged<TDto>(out int total, PagingParams<TEntity, TDto> pagingParams)
        {
            return base.FilterPaged<TEntity, TDto>(out total, pagingParams);
        }

        public virtual PagingResult<TDto> GetPaged<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class
        {
            return base.GetPaged<TEntity, TDto>(pagingParams);
        }

        public virtual async Task<PagingResult<TDto>> GetPagedAsync<TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TDto : class
        {
            return await base.GetPagedAsync<TEntity, TDto>(pagingParams);
        }

        public virtual bool Contain<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Contain<TEntity, TDto>(predicates);
        }

        public virtual async Task<bool> ContainAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.ContainAsync<TEntity, TDto>(predicates);
        }

        public virtual void Create<TDto>(params TDto[] dtos) where TDto : class
        {
            base.Create<TEntity, TDto>(dtos);
        }

        public virtual async Task CreateAsync<TDto>(params TDto[] dtos) where TDto : class
        {
            await base.CreateAsync<TEntity, TDto>(dtos);
        }

        public virtual int Delete<TKey>(TKey id)
        {
            return base.Delete<TEntity, TKey>(id);
        }

        public virtual int Delete<TKey>(TKey[] ids)
        {
            return base.Delete<TEntity, TKey>(ids);
        }

        public virtual int Delete<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Delete<TEntity, TDto>(predicates);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TKey id)
        {
            return await base.DeleteAsync<TEntity, TKey>(id);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TKey[] ids)
        {
            return await base.DeleteAsync<TEntity, TKey>(ids);
        }

        public virtual async Task<int> DeleteAsync<TDto>(params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.DeleteAsync<TEntity, TDto>(predicates);
        }

        public virtual int Update<TDto>(TDto dto, object id) where TDto : class
        {
            return base.Update<TEntity, TDto>(dto, id);
        }

        public virtual async Task<int> UpdateAsync<TDto>(TDto dto, object id) where TDto : class
        {
            return await base.UpdateAsync<TEntity, TDto>(dto, id);
        }

        public virtual int Update<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>
        {
            return base.Update<TEntity, TDto, TKey>(dtos);
        }

        public virtual async Task<int> UpdateAsync<TDto, TKey>(params TDto[] dtos) where TDto : class, IIdentifier<TKey>
        {
            return await base.UpdateAsync<TEntity, TDto, TKey>(dtos);
        }

        public virtual void Create<TDto>(TUserKey userId, params TDto[] dtos) where TDto : class
        {
            base.Create<TEntity, TDto>(userId, dtos);
        }

        public virtual async Task CreateAsync<TDto>(TUserKey userId, params TDto[] dtos) where TDto : class
        {
            await base.CreateAsync<TEntity, TDto>(userId, dtos);
        }

        public virtual int Delete<TKey>(TUserKey userId, TKey id)
        {
            return base.Delete<TEntity, TKey>(userId, id);
        }

        public virtual int Delete<TKey>(TUserKey userId, TKey[] ids)
        {
            return base.Delete<TEntity, TKey>(userId, ids);
        }

        public virtual int Delete<TDto>(TUserKey userId, params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return base.Delete<TEntity, TDto>(userId, predicates);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TUserKey userId, TKey id)
        {
            return await base.DeleteAsync<TEntity, TKey>(userId, id);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TUserKey userId, TKey[] ids)
        {
            return await base.DeleteAsync<TEntity, TKey>(userId, ids);
        }

        public virtual async Task<int> DeleteAsync<TDto>(TUserKey userId, params System.Linq.Expressions.Expression<Func<TDto, bool>>[] predicates)
        {
            return await base.DeleteAsync<TEntity, TDto>(userId, predicates);
        }

        public virtual int Update<TDto>(TUserKey userId, object id, TDto dto) where TDto : class
        {
            return base.Update<TEntity, TDto>(userId, id, dto);
        }

        public virtual async Task<int> UpdateAsync<TDto>(TUserKey userId, object id, TDto dto) where TDto : class
        {
            return await base.UpdateAsync<TEntity, TDto>(userId, id, dto);
        }

        public virtual int Update<TDto, TKey>(TUserKey userId, params TDto[] dtos) where TDto : class, IIdentifier<TKey>
        {
            return base.Update<TEntity, TDto, TKey>(userId, dtos);
        }

        public virtual async Task<int> UpdateAsync<TDto, TKey>(TUserKey userId, params TDto[] dtos) where TDto : class, IIdentifier<TKey>
        {
            return await base.UpdateAsync<TEntity, TDto, TKey>(userId, dtos);
        }

        #endregion
    }
}
