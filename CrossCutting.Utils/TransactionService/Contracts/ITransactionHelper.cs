using Microsoft.AspNetCore.Mvc.Filters;

namespace CrossCutting.Utils.TransactionService.Contracts
{
    public interface ITransactionHelper
    {
        void BeginTransaction();
        void EndTransaction(ActionExecutedContext filterContext);
        void CloseSession();
    }
}
