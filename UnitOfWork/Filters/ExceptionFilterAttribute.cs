

namespace UnitOfWork.Filters
{
    using CrossCutting.Utils.ExceptionService;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Text;

    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as NotFoundException;
                context.Exception = null;

                context.Result = new JsonResult(ex.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

                _logger.LogError(CreateMessageError(context, (int)HttpStatusCode.NotFound));
            }
            else if (context.Exception is BadRequestException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as BadRequestException;
                context.Exception = null;

                context.Result = new JsonResult(ex.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                _logger.LogError(CreateMessageError(context, (int)HttpStatusCode.BadRequest));
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                _logger.LogError(CreateMessageError(context, (int)HttpStatusCode.Unauthorized));
            }
            else if (context.Exception is ForbiddenException)
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                _logger.LogError(CreateMessageError(context, (int)HttpStatusCode.Forbidden));
            }
            else
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(CreateMessageError(context, (int)HttpStatusCode.InternalServerError));
            }

            base.OnException(context);
        }

        private static string CreateMessageError(ExceptionContext context, int codeError)
        {
            StringBuilder message = new StringBuilder();

            message.Append(Environment.NewLine);
            message.Append("Code Error: " + codeError.ToString() + Environment.NewLine);
            message.Append("URL: " + context.HttpContext.Request.Path.ToString() + Environment.NewLine);
            message.Append("Controller: " + context.ActionDescriptor.RouteValues["controller"] + Environment.NewLine);
            message.Append("Action: " + context.ActionDescriptor.RouteValues["action"] + Environment.NewLine);
            message.Append("Request: " + context.HttpContext.Request.QueryString + Environment.NewLine);
            message.Append("Error Message: " + context.Exception.Message + Environment.NewLine);
            message.Append("InnerException: " + context.Exception.InnerException + Environment.NewLine);
            message.Append("StackTrace: " + context.Exception.StackTrace + Environment.NewLine);

            return message.ToString();
        }
    }
}
