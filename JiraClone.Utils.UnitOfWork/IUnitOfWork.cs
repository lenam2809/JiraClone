using JiraClone.Utils.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity, TDto> GetRepository<TEntity, TDto>() where TEntity : class where TDto : class;
        Task<int> SaveChangesAsync();
    }
}
