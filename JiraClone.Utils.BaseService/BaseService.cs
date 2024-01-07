using AutoMapper;
using JiraClone.Utils.Repository;
using JiraClone.Utils.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.BaseService
{

    public abstract class BaseService<TEntity, TDto> : IBaseService<TDto> where TEntity : class where TDto : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual TDto GetById(int id)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().GetById(id);
        }

        public virtual IEnumerable<TDto> GetAll()
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().GetAll();
        }

        public virtual void Add(TDto dto)
        {
            _unitOfWork.GetRepository<TEntity, TDto>().Add(dto);
        }

        public virtual void Update(TDto dto)
        {
            _unitOfWork.GetRepository<TEntity, TDto>().Update(dto);
        }

        public virtual void Delete(int id)
        {
            _unitOfWork.GetRepository<TEntity, TDto>().Delete(id);
        }

        public virtual Task<TDto> GetByIdAsync(int id)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().GetByIdAsync(id);
        }

        public virtual Task<IEnumerable<TDto>> GetAllAsync()
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().GetAllAsync();
        }

        public virtual Task AddAsync(TDto dto)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().AddAsync(dto);
        }

        public virtual Task UpdateAsync(TDto dto)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().UpdateAsync(dto);
        }

        public virtual Task DeleteAsync(int id)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().DeleteAsync(id);
        }

        public virtual Task<TDto> FindAsync(Expression<Func<TDto, bool>> predicate)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().FindAsync(predicate);
        }

        public virtual Task<TDto> SingleOrDefaultAsync(Expression<Func<TDto, bool>> predicate)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().SingleOrDefaultAsync(predicate);
        }

        public virtual Task<int> CountAsync(Expression<Func<TDto, bool>> predicate)
        {
            return _unitOfWork.GetRepository<TEntity, TDto>().CountAsync(predicate);
        }


    }

}
