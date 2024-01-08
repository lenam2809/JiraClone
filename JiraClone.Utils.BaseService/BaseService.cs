using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using JiraClone.Utils.BaseService.Helpers;
using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.BaseService
{

    public abstract class BaseService : IBaseService, IDisposable
    {
        protected IRepository _repository;
        protected IMapper _mapper;

        public BaseService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual IQueryable<TDto> All<TEntity, TDto>() where TEntity : class
        {
            return _repository.All<TEntity>().ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>());
        }
        public virtual IQueryable<TDto> All<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping) where TEntity : class
        {
            return _repository.All<TEntity>().Select(mapping);
        }
        public virtual int Count<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            return ExpressionExtension.WhereMany<TDto>(_repository.All<TEntity>().ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>()), predicates).Count();
        }
        public async Task<int> CountAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            return await ExpressionExtension.WhereMany<TDto>(_repository.All<TEntity>().ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>()), predicates).CountAsync();

        }
        public virtual TDto Find<TEntity, TDto>(object id) where TEntity : class
        {
            return _mapper.Map<TEntity, TDto>(_repository.Find<TEntity>(id));
        }
        public virtual TDto Find<TEntity, TDto>(Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            TEntity source = _repository.Find<TEntity>(predicates);
            return _mapper.Map<TEntity, TDto>(source);
        }
        public virtual async Task<TDto> FindAsync<TEntity, TDto>(object id) where TEntity : class
        {
            TEntity source = await _repository.FindAsync<TEntity>(id);
            return _mapper.Map<TEntity, TDto>(source);
        }
        public virtual async Task<TDto> FindAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            TEntity entity = await _repository.FindAsync<TEntity>(predicates);
            return _mapper.Map<TEntity, TDto>(entity);
        }
        public virtual IQueryable<TDto> Filter<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            return ExpressionExtension.WhereMany<TDto>(_repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>()), predicates);
        }
        public virtual IQueryable<TDto> Filter<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping, params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            return ExpressionExtension.WhereMany<TDto>(_repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).Select(mapping), predicates);
        }
        public virtual PagingResult<TDto> FilterPaged<TEntity, TDto>(PagingParams<TDto> pagingParams) where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            PagingResult<TDto> obj = new PagingResult<TDto>
            {
                PageSize = pagingParams.ItemsPerPage,
                CurrentPage = pagingParams.Page
            };
            IQueryable<TDto> queryable = _repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>());
            List<Expression<Func<TDto, bool>>> predicates2 = pagingParams.GetPredicates();
            if (predicates2 != null && predicates2.Any())
            {
                queryable = ExpressionExtension.WhereMany<TDto>(queryable, (IEnumerable<Expression<Func<TDto, bool>>>)predicates2);
            }
            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null && pagingPredicates.Any())
            {
                queryable = ExpressionExtension.WhereMany<TDto>(queryable, pagingPredicates);
            }

            obj.TotalRows = queryable.Count();
            if (pagingParams.SortExpression != null)
            {
                queryable = queryable.OrderBy(pagingParams.SortExpression);
                if (pagingParams.StartingIndex > 0)
                {
                    queryable = queryable.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                queryable = queryable.Take(pagingParams.ItemsPerPage);
            }

            obj.Data = queryable.ToList();
            return obj;
        }
        public virtual PagingResult<TDto> FilterPaged<TEntity, TDto>(PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            PagingResult<TDto> obj = new PagingResult<TDto>
            {
                PageSize = pagingParams.ItemsPerPage,
                CurrentPage = pagingParams.Page
            };
            IQueryable<TDto> queryable = _repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>());
            List<Expression<Func<TDto, bool>>> predicates2 = pagingParams.GetPredicates();
            if (predicates2 != null && predicates2.Any())
            {
                queryable = ExpressionExtension.WhereMany<TDto>(queryable, (IEnumerable<Expression<Func<TDto, bool>>>)predicates2);
            }

            if (predicates != null && predicates.Any())
            {
                queryable = ExpressionExtension.WhereMany<TDto>(queryable, predicates);
            }

            obj.TotalRows = queryable.Count();
            if (pagingParams.SortExpression != null)
            {
                queryable = queryable.OrderBy(pagingParams.SortExpression);
                if (pagingParams.StartingIndex > 0)
                {
                    queryable = queryable.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                queryable = queryable.Take(pagingParams.ItemsPerPage);
            }

            obj.Data = queryable.ToList();
            return obj;
        }
        public virtual PagingResult<TDto> FilterPaged<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping, PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            PagingResult<TDto> obj = new PagingResult<TDto>
            {
                PageSize = pagingParams.ItemsPerPage,
                CurrentPage = pagingParams.Page
            };
            IQueryable<TDto> queryable = _repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).Select(mapping);
            List<Expression<Func<TDto, bool>>> predicates2 = pagingParams.GetPredicates();
            if (predicates2 != null && predicates2.Any())
            {
                queryable = ExpressionExtension.WhereMany<TDto>(queryable, (IEnumerable<Expression<Func<TDto, bool>>>)predicates2);
            }

            if (predicates != null && predicates.Any())
            {
                queryable = ExpressionExtension.WhereMany<TDto>(queryable, predicates);
            }

            obj.TotalRows = queryable.Count();
            if (pagingParams.SortExpression != null)
            {
                queryable = queryable.OrderBy(pagingParams.SortExpression);
                if (pagingParams.StartingIndex > 0)
                {
                    queryable = queryable.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                queryable = queryable.Take(pagingParams.ItemsPerPage);
            }

            obj.Data = queryable.ToList();
            return obj;
        }
        public virtual async Task<PagingResult<TDto>> FilterPagedAsync<TEntity, TDto>(PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            PagingResult<TDto> result = new PagingResult<TDto>
            {
                PageSize = pagingParams.ItemsPerPage,
                CurrentPage = pagingParams.Page
            };
            IQueryable<TDto> query = _repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>());
            List<Expression<Func<TDto, bool>>> predicates2 = pagingParams.GetPredicates();
            if (predicates2 != null && predicates2.Any())
            {
                query = ExpressionExtension.WhereMany<TDto>(query, (IEnumerable<Expression<Func<TDto, bool>>>)predicates2);
            }

            if (predicates != null && predicates.Any())
            {
                query = ExpressionExtension.WhereMany<TDto>(query, predicates);
            }

            PagingResult<TDto> pagingResult = result;
            pagingResult.TotalRows = await query.CountAsync();
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);
                if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }

            pagingResult = result;
            pagingResult.Data = await query.ToListAsync();
            return result;
        }
        public virtual async Task<PagingResult<TDto>> FilterPagedAsync<TEntity, TDto>(Expression<Func<TEntity, TDto>> mapping, PagingParams<TDto> pagingParams, params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            PagingResult<TDto> result = new PagingResult<TDto>
            {
                PageSize = pagingParams.ItemsPerPage,
                CurrentPage = pagingParams.Page
            };
            IQueryable<TDto> query = _repository.Filter(Array.Empty<Expression<Func<TEntity, bool>>>()).Select(mapping);
            List<Expression<Func<TDto, bool>>> predicates2 = pagingParams.GetPredicates();
            if (predicates2 != null && predicates2.Any())
            {
                query = ExpressionExtension.WhereMany<TDto>(query, (IEnumerable<Expression<Func<TDto, bool>>>)predicates2);
            }

            if (predicates != null && predicates.Any())
            {
                query = ExpressionExtension.WhereMany<TDto>(query, predicates);
            }

            PagingResult<TDto> pagingResult = result;
            pagingResult.TotalRows = await query.CountAsync();
            if (pagingParams.SortExpression != null)
            {
                query = query.OrderBy(pagingParams.SortExpression);
                if (pagingParams.StartingIndex > 0)
                {
                    query = query.Skip(pagingParams.StartingIndex);
                }
            }

            if (pagingParams.ItemsPerPage != -1 && pagingParams.ItemsPerPage <= 0)
            {
                pagingParams.ItemsPerPage = 100;
            }

            if (pagingParams.ItemsPerPage > 0)
            {
                query = query.Take(pagingParams.ItemsPerPage);
            }

            pagingResult = result;
            pagingResult.Data = await query.ToListAsync();
            return result;
        }
        public virtual bool Contain<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            return ExpressionExtension.WhereMany<TDto>(_repository.All<TEntity>().ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>()), predicates).Any();
        }
        public virtual async Task<bool> ContainAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            return await ExpressionExtension.WhereMany<TDto>(_repository.All<TEntity>().ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TDto, object>>>()), predicates).AnyAsync();
        }
        public virtual void Create<TEntity, TDto>(params TDto[] dtos) where TEntity : class where TDto : class
        {
            TEntity[] array = _mapper.Map<TDto[], TEntity[]>(dtos);
            _repository.Create(array);
            if (_mapper.ConfigurationProvider.Internal().FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    _mapper.Map(array[i], dtos[i]);
                }
            }
        }
        public virtual void Create<TEntity, TDto>(Func<TDto, TEntity> mapping, params TDto[] dtos) where TEntity : class where TDto : class
        {
            TEntity[] array = dtos.Select(mapping).ToArray();
            _repository.Create(array);
            if (_mapper.ConfigurationProvider.Internal().FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    _mapper.Map(array[i], dtos[i]);
                }
            }
        }
        public virtual async Task CreateAsync<TEntity, TDto>(params TDto[] dtos) where TEntity : class where TDto : class
        {
            TEntity[] entities = _mapper.Map<TDto[], TEntity[]>(dtos);
            await _repository.CreateAsync(entities);
            if (_mapper.ConfigurationProvider.Internal().FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    _mapper.Map(entities[i], dtos[i]);
                }
            }
        }
        public virtual async Task CreateAsync<TEntity, TDto>(Func<TDto, TEntity> mapping, params TDto[] dtos) where TEntity : class where TDto : class
        {
            TEntity[] entities = dtos.Select(mapping).ToArray();
            await _repository.CreateAsync(entities);
            if (_mapper.ConfigurationProvider.Internal().FindTypeMapFor<TEntity, TDto>() != null)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    _mapper.Map(entities[i], dtos[i]);
                }
            }
        }
        public virtual int Delete<TEntity, TKey>(TKey id) where TEntity : class
        {
            return _repository.Delete<TEntity, TKey>(id);
        }
        public virtual int Delete<TEntity, TKey>(TKey[] ids) where TEntity : class
        {
            return _repository.Delete<TEntity, TKey>(ids);
        }
        public virtual int Delete<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            List<TDto> source = Filter<TEntity, TDto>(predicates).ToList();
            return _repository.Delete(_mapper.Map<List<TDto>, List<TEntity>>(source).ToArray());
        }
        public virtual async Task<int> DeleteAsync<TEntity, TKey>(TKey id) where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity, TKey>(id);
        }
        public virtual async Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids) where TEntity : class
        {
            return await _repository.DeleteAsync<TEntity, TKey>(ids);
        }
        public virtual async Task<int> DeleteAsync<TEntity, TDto>(params Expression<Func<TDto, bool>>[] predicates) where TEntity : class
        {
            List<TDto> source = Filter<TEntity, TDto>(predicates).ToList();
            return await _repository.DeleteAsync(_mapper.Map<List<TDto>, List<TEntity>>(source).ToArray());
        }
        public virtual int Update<TEntity, TDto>(object id, TDto dto) where TEntity : class where TDto : class
        {
            TEntity val = _repository.Find<TEntity>(id);
            _mapper.Map(dto, val);
            return _repository.Update<TEntity>(val);
        }
        public virtual int Update<TEntity, TDto>(Action<TDto, TEntity> mapping, object id, TDto dto) where TEntity : class where TDto : class
        {
            TEntity val = _repository.Find<TEntity>(id);
            mapping(dto, val);
            return _repository.Update<TEntity>(val);
        }
        public virtual async Task<int> UpdateAsync<TEntity, TDto>(object id, TDto dto) where TEntity : class where TDto : class
        {
            TEntity val = await _repository.FindAsync<TEntity>(id);
            _mapper.Map(dto, val);
            return await _repository.UpdateAsync<TEntity>(val);
        }
        public virtual async Task<int> UpdateAsync<TEntity, TDto>(Action<TDto, TEntity> mapping, object id, TDto dto) where TEntity : class where TDto : class
        {
            TEntity val = await _repository.FindAsync<TEntity>(id);
            mapping(dto, val);
            return await _repository.UpdateAsync<TEntity>(val);
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
        public virtual void Dispose()
        {
            if (_repository != null)
            {
                _repository.Dispose();
                _repository = null;
            }
        }
    }
}
