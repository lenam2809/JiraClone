﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository
{
    public interface IRepository<TEntity, TDto> where TEntity : class where TDto : class
    {
        TDto GetById(int id);
        IEnumerable<TDto> GetAll();
        void Add(TDto dto);
        void Update(TDto dto);
        void Delete(int id);

        #region Async Methods
        Task<TDto> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task AddAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(int id);

        Task<TDto> FindAsync(Expression<Func<TDto, bool>> predicate);
        Task<TDto> SingleOrDefaultAsync(Expression<Func<TDto, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TDto, bool>> predicate);

        #endregion
    }
}
