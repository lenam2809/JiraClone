using JiraClone.Utils.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity, TDto> GetRepository<TEntity, TDto>() 
            where TEntity : class 
            where TDto : class
        {
            var type = typeof(IRepository<TEntity, TDto>);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(typeof(TEntity), typeof(TDto));
                var repositoryInstance = Activator.CreateInstance(repositoryType, _dbContext);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity, TDto>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }


}
