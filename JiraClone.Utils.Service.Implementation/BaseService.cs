using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Audit;
using JiraClone.Utils.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Service.Implementation
{
    public class BaseService : IBaseService
    {
        protected IRepository _repository;

        public BaseService(IRepository repository)
        {
            _repository = repository;
        }

        #region IBaseService Members

        public virtual IQueryable<TDto> All<TEntity, TDto>()
            where TEntity : class
        {
            return _repository.All<TEntity>().Project().To<TDto>();
        }

        public virtual int Count<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return _repository.Count<TEntity>(GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual async Task<int> CountAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return await _repository.CountAsync<TEntity>(GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual TDto Find<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return Mapper.Map<TEntity, TDto>(_repository.Find<TEntity>(GetPredicates<TEntity, TDto>(predicates)));
        }

        public virtual TDto Find<TEntity, TDto>(object id)
            where TEntity : class
        {
            var entity = _repository.Find<TEntity>(id);
            var dto = Mapper.Map<TEntity, TDto>(entity);
            return dto;
        }

        public virtual async Task<TDto> FindAsync<TEntity, TDto>(object id)
            where TEntity : class
        {
            return Mapper.Map<TEntity, TDto>(await _repository.FindAsync<TEntity>(id));
        }

        public virtual async Task<TDto> FindAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return Mapper.Map<TEntity, TDto>(await _repository.FindAsync<TEntity>(GetPredicates<TEntity, TDto>(predicates)));
        }

        public virtual IQueryable<TDto> Filter<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return _repository.Filter<TEntity>().Project().To<TDto>().WhereMany(predicates);
        }

        public virtual IQueryable<TDto> FilterPaged<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            IQueryable<TDto> query = _repository.Filter<TEntity>().Project().To<TDto>();

            if (pagingParams.Predicates != null)
            {
                query = query.WhereMany(pagingParams.Predicates.ToArray());
            }

            // Ordering
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);

                // Skipping only work after ordering
                if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.PageSize > 0)
            {
                query = query.Take(pagingParams.PageSize);
            }

            return query.AsQueryable();

            //return _repository.FilterPaged<TEntity>(ToRepoParams(pagingParams)).Project().To<TDto>();
        }

        public virtual IQueryable<TDto> FilterPaged<TEntity, TDto>(out int total, PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            IQueryable<TDto> query = _repository.Filter<TEntity>().Project().To<TDto>();

            if (pagingParams.Predicates != null)
            {
                query = query.WhereMany(pagingParams.Predicates.ToArray());
            }

            total = query.Count();

            // Ordering
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);

                // Skipping only work after ordering
                if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.PageSize > 0)
            {
                query = query.Take(pagingParams.PageSize);
            }

            return query.AsQueryable();

            //return _repository.FilterPaged<TEntity>(out total, ToRepoParams(pagingParams)).Project().To<TDto>();
        }

        public virtual PagingResult<TDto> GetPaged<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
            where TDto : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            var result = new PagingResult<TDto>();
            result.Page = pagingParams.Page;
            result.PageSize = pagingParams.PageSize;

            IQueryable<TDto> query = _repository.Filter<TEntity>().Project().To<TDto>();

            if (pagingParams.Predicates != null)
            {
                query = query.WhereMany(pagingParams.Predicates.ToArray());
            }

            result.Count = query.Count();

            if (result.Count > 0)
            {
                // Ordering
                if (pagingParams.SortExpression != null)
                {
                    query = query.OrderBy(pagingParams.SortExpression);

                    // Skipping only work after ordering
                    if (pagingParams.StartingIndex > 0)
                    {
                        query = query.Skip(pagingParams.StartingIndex);
                    }
                }
                result.Items = query.Take(pagingParams.PageSize).ToList();
            }

            return result;
        }

        public virtual async Task<PagingResult<TDto>> GetPagedAsync<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
            where TDto : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            IQueryable<TDto> query = _repository.Filter<TEntity>().Project().To<TDto>();

            if (pagingParams.Predicates != null)
            {
                query = query.WhereMany(pagingParams.Predicates.ToArray());
            }

            var total = await query.CountAsync();

            // Ordering
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);

                // Skipping only work after ordering
                if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            var result = new PagingResult<TDto>();

            result.Items = await query.Take(pagingParams.PageSize).ToListAsync();
            result.Count = total;
            result.Page = pagingParams.Page;
            result.PageSize = pagingParams.PageSize;

            return result;
        }

        public virtual bool Contain<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return _repository.Contain<TEntity>(GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual async Task<bool> ContainAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return await _repository.ContainAsync<TEntity>(GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual void Create<TEntity, TDto>(params TDto[] dtos)
            where TEntity : class
            where TDto : class
        {
            var entities = Mapper.Map<TDto[], TEntity[]>(dtos);

            _repository.Create<TEntity>(entities);

            if (dtos[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)) && entities[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    dtos[i].SetProperty("Id", entities[i].GetProperty("Id"));
                }
            }

            if (Mapper.FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    Mapper.Map(entities[i], dtos[i]);
                }
            }
        }

        public virtual async Task CreateAsync<TEntity, TDto>(params TDto[] dtos)
            where TEntity : class
            where TDto : class
        {
            var entities = Mapper.Map<TDto[], TEntity[]>(dtos);

            await _repository.CreateAsync<TEntity>(entities);

            if (dtos[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)) && entities[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    dtos[i].SetProperty("Id", entities[i].GetProperty("Id"));
                }
            }

            if (Mapper.FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    Mapper.Map(entities[i], dtos[i]);
                }
            }
        }

        public virtual int Delete<TEntity, TKey>(TKey id)
            where TEntity : class
        {
            return _repository.Delete<TEntity, TKey>(id);
        }

        public virtual int Delete<TEntity, TKey>(TKey[] ids)
            where TEntity : class
        {
            return _repository.Delete<TEntity, TKey>(ids);
        }

        public virtual int Delete<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return _repository.Delete<TEntity>(GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual async Task<int> DeleteAsync<TEntity, TKey>(TKey id)
            where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity, TKey>(id);
        }

        public virtual async Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids)
            where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity, TKey>(ids);
        }

        public virtual async Task<int> DeleteAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity>(GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual int Update<TEntity, TDto>(TDto dto, object id)
            where TEntity : class
            where TDto : class
        {
            var entity = _repository.Find<TEntity>(id);
            Mapper.Map<TDto, TEntity>(dto, entity);

            return _repository.Update<TEntity>(entity);
        }

        public virtual async Task<int> UpdateAsync<TEntity, TDto>(TDto dto, object id)
            where TEntity : class
            where TDto : class
        {
            var entity = await _repository.FindAsync<TEntity>(id);
            Mapper.Map<TDto, TEntity>(dto, entity);

            return await _repository.UpdateAsync<TEntity>(entity);
        }

        public virtual int Update<TEntity, TDto, TKey>(params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>
        {
            var entitiyIds = dtos.Select(x => x.Id).ToList();
            var entities = new List<TEntity>();
            foreach (var dto in dtos)
            {
                var entity = _repository.Find<TEntity>(dto.Id);
                Mapper.Map<TDto, TEntity>(dto, entity);

                entities.Add(entity);
            }

            return _repository.Update<TEntity>(entities.ToArray());
        }

        public virtual async Task<int> UpdateAsync<TEntity, TDto, TKey>(params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>
        {
            var entitiyIds = dtos.Select(x => x.Id).ToList();
            var entities = new List<TEntity>();
            foreach (var dto in dtos)
            {
                var entity = await _repository.FindAsync<TEntity>(dto.Id);
                Mapper.Map<TDto, TEntity>(dto, entity);

                entities.Add(entity);
            }

            return await _repository.UpdateAsync<TEntity>(entities.ToArray());
        }

        public virtual int ExecuteNonQuery(string sql, params object[] sqlParams)
        {
            return _repository.ExecuteNonQuery(sql, sqlParams);
        }

        public virtual TResult ExecuteReader<TResult>(string sql, params object[] sqlParams)
        {
            return _repository.ExecuteReader<TResult>(sql, sqlParams);
        }

        public virtual TContext GetDbContext<TContext>() where TContext : class
        {
            return _repository.GetDbContext<TContext>();
        }

        public virtual ITransaction BeginTransaction()
        {
            return _repository.BeginTransaction();
        }

        public virtual ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _repository.BeginTransaction(isolationLevel);
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (_repository != null)
            {
                _repository.Dispose();
                _repository = null;
            }
        }

        #endregion

        #region Helpers

        protected Expression<Func<TEntity, bool>>[] GetPredicates<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates)
        {
            return MappingHelper.GetMappedSelectors<TDto, TEntity, bool>(predicates).ToArray();
        }

        protected PagingParams<TEntity> ToRepoParams<TEntity, TDto>(PagingParams<TEntity, TDto> pagingParams)
            where TEntity : class
            where TDto : class
        {
            var result = new PagingParams<TEntity>()
            {
                Page = pagingParams.Page,
                PageSize = pagingParams.PageSize,
                SortExpression = ConvertExpression<TEntity, TDto>(pagingParams.SortExpression),
                Predicates = GetPredicates<TEntity, TDto>(pagingParams.Predicates.ToArray()).ToList()
            };

            return result;
        }

        private string ConvertExpression<TEntity, TDto>(string dto)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

    public class BaseService<TUserKey> : BaseService, IBaseService<TUserKey> where TUserKey : struct
    {
        protected new IRepository<TUserKey> _repository;

        public BaseService(IRepository<TUserKey> repository)
            : base(repository)
        {
            _repository = repository;
        }

        #region IBaseService<TUserKey> Members

        public void SetBatchInsert(bool inBatchInsert)
        {
            _repository.SetBatchInsert(inBatchInsert);
        }

        public void Commit()
        {
            _repository.Commit();
        }
        public virtual void Create<TEntity, TDto>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class
        {
            var entities = Mapper.Map<TDto[], TEntity[]>(dtos);

            _repository.Create<TEntity>(userId, entities);

            if (dtos[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)) && entities[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    dtos[i].SetProperty("Id", entities[i].GetProperty("Id"));
                }
            }

            if (Mapper.FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    Mapper.Map(entities[i], dtos[i]);
                }
            }
        }

        public virtual async Task CreateAsync<TEntity, TDto>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class
        {
            var entities = Mapper.Map<TDto[], TEntity[]>(dtos);

            await _repository.CreateAsync<TEntity>(userId, entities);

            if (dtos[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)) && entities[0].IsDerivedFromGenericInterface(typeof(IIdentifier<>)))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    dtos[i].SetProperty("Id", entities[i].GetProperty("Id"));
                }
            }

            if (Mapper.FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    Mapper.Map(entities[i], dtos[i]);
                }
            }
        }

        public virtual int Delete<TEntity, TKey>(TUserKey userId, TKey id)
            where TEntity : class
        {
            return _repository.Delete<TEntity, TKey>(userId, id);
        }

        public virtual int Delete<TEntity, TKey>(TUserKey userId, TKey[] ids)
            where TEntity : class
        {
            return _repository.Delete<TEntity, TKey>(userId, ids);
        }

        public virtual int Delete<TEntity, TDto>(TUserKey userId, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return _repository.Delete<TEntity>(userId, GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual async Task<int> DeleteAsync<TEntity, TKey>(TUserKey userId, TKey id)
            where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity, TKey>(userId, id);
        }

        public virtual async Task<int> DeleteAsync<TEntity, TKey>(TUserKey userId, TKey[] ids)
            where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity, TKey>(userId, ids);
        }

        public virtual async Task<int> DeleteAsync<TEntity, TDto>(TUserKey userId, params Expression<Func<TDto, bool>>[] predicates)
            where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity>(userId, GetPredicates<TEntity, TDto>(predicates));
        }

        public virtual int Update<TEntity, TDto>(TUserKey userId, object id, TDto dto)
            where TEntity : class
            where TDto : class
        {
            var entity = _repository.Find<TEntity>(id);
            Mapper.Map<TDto, TEntity>(dto, entity);

            return _repository.Update<TEntity>(userId, entity);
        }

        public virtual async Task<int> UpdateAsync<TEntity, TDto>(TUserKey userId, object id, TDto dto)
            where TEntity : class
            where TDto : class
        {
            var entity = await _repository.FindAsync<TEntity>(id);
            Mapper.Map<TDto, TEntity>(dto, entity);

            return await _repository.UpdateAsync<TEntity>(userId, entity);
        }

        public virtual int Update<TEntity, TDto, TKey>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>
        {
            var entitiyIds = dtos.Select(x => x.Id).ToList();
            var entities = new List<TEntity>();
            foreach (var dto in dtos)
            {
                var entity = _repository.Find<TEntity>(dto.Id);
                Mapper.Map<TDto, TEntity>(dto, entity);

                entities.Add(entity);
            }

            return _repository.Update<TEntity>(userId, entities.ToArray());
        }

        public virtual async Task<int> UpdateAsync<TEntity, TDto, TKey>(TUserKey userId, params TDto[] dtos)
            where TEntity : class
            where TDto : class, IIdentifier<TKey>
        {
            var entitiyIds = dtos.Select(x => x.Id).ToList();
            var entities = new List<TEntity>();
            foreach (var dto in dtos)
            {
                var entity = await _repository.FindAsync<TEntity>(dto.Id);
                Mapper.Map<TDto, TEntity>(dto, entity);

                entities.Add(entity);
            }

            return await _repository.UpdateAsync<TEntity>(userId, entities.ToArray());
        }

        #endregion
    }
}
