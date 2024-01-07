using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using JiraClone.Utils.Repository;
using JiraClone.Utils.Repository.Helpers;
using JiraClone.Utils.Repository.Repository;
using JiraClone.Utils.Repository.Transaction;
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
        public virtual int Count<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            return _repository.Count<TEntity>(predicates);
        }
        public async Task<int> CountAsync<TEntity, TDto>(Expression<Func<TEntity, bool>>[] predicate) where TEntity : class
        {
            return await _repository.CountAsync<TEntity>(predicate);
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


        public virtual IQueryable<TDto> Filter<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            IQueryable<TEntity> source = _repository.Filter<TEntity>(predicates);
            IQueryable<TDto> mappedResult = source.ProjectTo<TDto>(_mapper.ConfigurationProvider);
            return mappedResult;
        }

        public virtual PagingResult<TDto> FilterPaged<TEntity, TDto>(PagingParams<TDto> pagingParams) where TEntity : class
        {
            var entities = Mapper.Map<TDto[], TEntity[]>(pagingParams);
            var source = _repository.FilterPaged<TEntity>(out long totalCount, pagingParams);
            var mappedResult = source.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToList();

            return new PagingResult<TDto>
            {
                TotalRows = totalCount,
                Data = mappedResult,
                CurrentPage = pagingParams.Page,
                PageSize = pagingParams.ItemsPerPage
            };
        }

        public virtual bool Contain<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            return _repository.Contain<TEntity>(predicates);
        }


        public virtual async Task<bool> ContainAsync<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
        {
            return await _repository.ContainAsync<TEntity>(predicates);
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

        public virtual int Delete<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
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

        public virtual async Task<int> DeleteAsync<TEntity, TDto>(params Expression<Func<TEntity, bool>>[] predicates) where TEntity : class
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
