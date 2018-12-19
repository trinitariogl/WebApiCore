

namespace CrossCutting.Utils.TransactionService
{
    using CrossCutting.Utils.TransactionService.Contracts;
    using GenericRepository.Transaction;
    using GenericRepository.UnitOfWork;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;

    public class TransactionHelper : ITransactionHelper
    {
        private IUnitOfWork _uow;
        private ITransaction _tx;

        //private readonly ILogger _log;

        public TransactionHelper(IUnitOfWork uow) //, ILogger log)
        {
            _uow = uow;
            //_log = log;
        }

        private bool TransactionHandled { get; set; }
        private bool SessionClosed { get; set; }

        public void BeginTransaction()
        {
            _tx = _uow.BeginTransaction();
        }
        public void EndTransaction(ActionExecutedContext filterContext)
        {
            if (_tx == null) throw new NotSupportedException();
            if (filterContext.Exception == null)
            {
                _uow.SaveChangesAsync();
                _tx.Commit();
            }
            else
            {
                try
                {
                    _tx.Rollback();
                }
                catch (Exception ex)
                {
                    throw new AggregateException(filterContext.Exception, ex);
                }

            }

            TransactionHandled = true;
        }

        public void CloseSession()
        {
            if (_tx != null)
            {
                _tx.Dispose();
                _tx = null;
            }

            if (_uow != null)
            {
                _uow.Dispose();
                _uow = null;
            }

            SessionClosed = true;
        }
    }
}
