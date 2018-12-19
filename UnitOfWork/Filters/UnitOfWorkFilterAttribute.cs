
namespace UnitOfWork.Filters
{
    using CrossCutting.Utils.TransactionService.Contracts;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class UnitOfWorkFilterAttribute : ActionFilterAttribute
    {
        private readonly ITransactionHelper _helper;

        public UnitOfWorkFilterAttribute(ITransactionHelper helper)
        {
            _helper = helper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _helper.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            _helper.EndTransaction(actionExecutedContext);
            _helper.CloseSession();
        }
    }
}
