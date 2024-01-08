using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Utils.Repository
{
    public class Transaction : ITransaction
    {
        private IDbContextTransaction _dbContextTransaction;

        protected internal Transaction(IDbContextTransaction dbContextTransaction)
        {
            // TODO: Complete member initialization
            _dbContextTransaction = dbContextTransaction;
        }

        #region ITransaction Members

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }

        #endregion
    }
}
