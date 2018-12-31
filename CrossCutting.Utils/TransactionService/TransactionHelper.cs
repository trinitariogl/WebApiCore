

namespace CrossCutting.Utils.TransactionService
{
    using CrossCutting.Utils.TransactionService.Contracts;
    using GenericRepository.Transaction;
    using GenericRepository.UnitOfWork;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System;

    public class TransactionHelper : ITransactionHelper
    {
        private IUnitOfWork _uow;
        private ITransaction _tx;
        private ILogger<TransactionHelper> _logger;

        public TransactionHelper(IUnitOfWork uow, ILogger<TransactionHelper> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        private bool TransactionHandled { get; set; }
        private bool SessionClosed { get; set; }

        public void BeginTransaction()
        {
            _logger.LogInformation("Entra en begin transaction");
            _tx = _uow.BeginTransaction();
        }
        public void EndTransaction(ActionExecutedContext filterContext)
        {
            if (_tx == null) throw new NotSupportedException();
            if (filterContext.Exception == null)
            {
                _logger.LogInformation("Entra en begin End Transaction");
                //_uow.SaveChangesAsync();
                //_uow.SaveEntitiesAsync();
                _tx.Commit();
            }
            else
            {
                try
                {
                    _logger.LogInformation("Entra en begin RollBack");
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
