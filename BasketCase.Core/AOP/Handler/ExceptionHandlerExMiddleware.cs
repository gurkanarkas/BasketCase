using Core.Models.HandlerModels;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BasketCase.Core.AOP.Handler
{
    public class ExceptionHandlerExMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerExMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ErrorResponse();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var details = new ErrorDetails()
            {
                statusCode = context.Response.StatusCode,
                userFriendlyMessage = "A technical error has occurred. You can use the Query Code field so that we can examine the details of the error.", // This message will stored in localization file or service.
                queryCode = Guid.NewGuid().ToString(), // This property will use for cloud logging. 
            };

            response.Model = details;
            response.StatusCode = HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(response.Model.ToString());
        }
    }
}
