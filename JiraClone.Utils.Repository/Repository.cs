using AutoMapper;
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
    public class Repository<TEntity, TDto> : IRepository<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public Repository(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public TDto GetById(int id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            return _mapper.Map<TEntity, TDto>(entity);
        }

        public IEnumerable<TDto> GetAll()
        {
            var entities = _dbContext.Set<TEntity>().ToList();
            return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(entities);
        }

        public void Add(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Update(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);
            _dbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(int id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            _dbContext.Set<TEntity>().Remove(entity);
        }

        #region Async Methods

        public async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            return _mapper.Map<TEntity, TDto>(entity);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _dbContext.Set<TEntity>().ToListAsync();
            return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(entities);
        }

        public async Task AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);
            _dbContext.Set<TEntity>().Update(entity);
            await Task.CompletedTask; // Entity Framework tự động tracking thay đổi.
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            _dbContext.Set<TEntity>().Remove(entity);
        }


        public async Task<TDto> FindAsync(Expression<Func<TDto, bool>> predicate)
        {
            var entityPredicate = _mapper.Map<Expression<Func<TDto, bool>>, Expression<Func<TEntity, bool>>>(predicate);
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(entityPredicate);

            return _mapper.Map<TEntity, TDto>(entity);
        }

        public async Task<TDto> SingleOrDefaultAsync(Expression<Func<TDto, bool>> predicate)
        {
            var entityPredicate = _mapper.Map<Expression<Func<TDto, bool>>, Expression<Func<TEntity, bool>>>(predicate);
            var entity = await _dbContext.Set<TEntity>().SingleOrDefaultAsync(entityPredicate);

            return _mapper.Map<TEntity, TDto>(entity);
        }

        public async Task<int> CountAsync(Expression<Func<TDto, bool>> predicate)
        {
            var entityPredicate = _mapper.Map<Expression<Func<TDto, bool>>, Expression<Func<TEntity, bool>>>(predicate);
            return await _dbContext.Set<TEntity>().CountAsync(entityPredicate);
        }

        #endregion
    }

}
