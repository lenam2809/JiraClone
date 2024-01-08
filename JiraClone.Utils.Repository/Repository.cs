using JiraClone.Utils.Repository.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository
{
    public class Repository<TContext> : IRepository where TContext : DbContext, new()
    {
        protected TContext _dbContext;
        protected bool _inBatchInsert = false;
        public Repository(TContext dbContext = null)
        {
            _dbContext = dbContext ?? new TContext();
        }

        public virtual IQueryable<TEntity> All<TEntity>()
            where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual int Count<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return query.Count();
        }

        public async virtual Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return await query.CountAsync();
        }

        public virtual TEntity Find<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return query.FirstOrDefault();
        }

        public virtual TEntity Find<TEntity>(object id)
            where TEntity : class
        {
            if (id is object[] compositeKeyValues)
            {
                var entity = _dbContext.Set<TEntity>().Find(compositeKeyValues);
                return entity;
            }
            else
            {
                return _dbContext.Set<TEntity>().Find(id);
            }
        }

        public async virtual Task<TEntity> FindAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async virtual Task<TEntity> FindAsync<TEntity>(object id)
            where TEntity : class
        {
            if (id is object[])
            {
                return await _dbContext.Set<TEntity>().FindAsync(id as object[]);
            }
            else
            {
                return await _dbContext.Set<TEntity>().FindAsync(id);
            }
        }

        public virtual IQueryable<TEntity> Filter<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (predicates != null)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            return query;
        }

        public virtual IQueryable<TEntity> FilterPaged<TEntity>(PagingParams<TEntity> pagingParams)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException(nameof(pagingParams));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null)
            {
                foreach (var predicate in pagingPredicates)
                {
                    query = query.Where(predicate);
                }
            }

            // Sorting
            if (pagingParams.SortExpression != null)
            {
                query = ApplyOrdering(query, pagingParams.SortExpression, pagingParams.SortDirection);

                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
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

            return query;
        }

        public virtual IQueryable<TEntity> FilterPaged<TEntity>(out long total, PagingParams<TEntity> pagingParams)
            where TEntity : class
        {
            if (pagingParams == null)
            {
                throw new ArgumentNullException(nameof(pagingParams));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            var pagingPredicates = pagingParams.GetPredicates();
            if (pagingPredicates != null)
            {
                foreach (var predicate in pagingPredicates)
                {
                    query = query.Where(predicate);
                }
            }

            total = query.Count(); // Get the total count before pagination

            // Sorting
            if (pagingParams.SortExpression != null)
            {
                query = ApplyOrdering(query, pagingParams.SortExpression, pagingParams.SortDirection);

                // Paging after sorting
                if (pagingParams.Start > 0)
                {
                    query = query.Skip(pagingParams.Start);
                }
                // Skipping only work after ordering
                else if (pagingParams.StartingIndex > 0)
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

            return query;
        }

        public bool Contain<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            if (predicates == null || predicates.Length == 0)
            {
                throw new ArgumentException("At least one predicate must be provided.", nameof(predicates));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return query.Any();
        }

        public async Task<bool> ContainAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            if (predicates == null || predicates.Length == 0)
            {
                throw new ArgumentException("At least one predicate must be provided.", nameof(predicates));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return await query.AnyAsync();
        }

        public virtual void Create<TEntity>(params TEntity[] entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                _dbContext.Set<TEntity>().Add(entity);
            }
            if (!_inBatchInsert)
            {
                _dbContext.SaveChanges();
            }
        }

        public async virtual Task CreateAsync<TEntity>(params TEntity[] entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                _dbContext.Set<TEntity>().Add(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

        public virtual int Delete<TEntity, TKey>(TKey id)
            where TEntity : class
        {
            var entity = _dbContext.Find<TEntity>(id);

            if (entity != null)
            {
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }

                _dbContext.Set<TEntity>().Remove(entity);
                return _dbContext.SaveChanges();
            }
            else
            {
                // Xử lý logic khi không tìm thấy entity
                return 0; // Hoặc throw một exception phù hợp
            }
        }

        public virtual int Delete<TEntity, TKey>(TKey[] ids)
            where TEntity : class
        {
            foreach (var id in ids)
            {
                var entity = Find<TEntity>(id);
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }
                _dbContext.Set<TEntity>().Remove(entity);
            }

            return _dbContext.SaveChanges();
        }

        public virtual int Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity is ICascadeDelete<TContext>)
            {
                (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
            }
            _dbContext.Set<TEntity>().Remove(entity);

            return _dbContext.SaveChanges();
        }

        public virtual int Delete<TEntity>(TEntity[] entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }
                _dbContext.Set<TEntity>().Remove(entity);
            }

            return _dbContext.SaveChanges();
        }

        public virtual int Delete<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            var entiteis = Filter(predicates);
            foreach (var entity in entiteis)
            {
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }
                _dbContext.Set<TEntity>().Remove(entity);
            }
            return _dbContext.SaveChanges();
        }

        public async virtual Task<int> DeleteAsync<TEntity, TKey>(TKey id)
            where TEntity : class
        {
            var entity = await FindAsync<TEntity>(id);
            if (entity is ICascadeDelete<TContext>)
            {
                (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
            }
            _dbContext.Set<TEntity>().Remove(entity);

            return await _dbContext.SaveChangesAsync();
        }

        public async virtual Task<int> DeleteAsync<TEntity, TKey>(TKey[] ids)
            where TEntity : class
        {
            foreach (var id in ids)
            {
                var entity = await FindAsync<TEntity>(id);
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }
                _dbContext.Set<TEntity>().Remove(entity);
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async virtual Task<int> DeleteAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity is ICascadeDelete<TContext>)
            {
                (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
            }
            _dbContext.Set<TEntity>().Remove(entity);

            return await _dbContext.SaveChangesAsync();
        }

        public async virtual Task<int> DeleteAsync<TEntity>(TEntity[] entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }
                _dbContext.Set<TEntity>().Remove(entity);
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async virtual Task<int> DeleteAsync<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
            where TEntity : class
        {
            var entities = Filter(predicates);
            foreach (var entity in entities)
            {
                if (entity is ICascadeDelete<TContext>)
                {
                    (entity as ICascadeDelete<TContext>).OnDelete(_dbContext);
                }
                _dbContext.Set<TEntity>().Remove(entity);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public virtual int Update<TEntity>(params TEntity[] entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                var entry = _dbContext.Entry(entity);
            }
            return _dbContext.SaveChanges();
        }

        public async virtual Task<int> UpdateAsync<TEntity>(params TEntity[] entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                var entry = _dbContext.Entry(entity);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async virtual Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public TDbContext GetDbContext<TDbContext>()
            where TDbContext : class
        {
            return _dbContext as TDbContext;
        }

        public virtual ITransaction BeginTransaction()
        {
            return new Transaction(_dbContext.Database.BeginTransaction());
        }

        public virtual ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new Transaction(_dbContext.Database.BeginTransaction(isolationLevel));
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

        private IQueryable<TEntity> ApplyOrdering<TEntity, TKey>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, TKey>> sortExpression,
        SortDirection sortDirection)
        where TEntity : class
        {
            return sortDirection == SortDirection.Ascending
                ? query.OrderBy(sortExpression)
                : query.OrderByDescending(sortExpression);
        }
    }
}
