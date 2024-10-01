using ECommerce.APP.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ECommerce.APP.SharedLibrary.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            var title = "Error";
            var message = "An internal error occur, kindly try again later";
            var statusCode = (int)HttpStatusCode.InternalServerError;

            try
            {
                await next(context);
                if(context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request attempt";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //log exceptions to file, console, debug
                LogExceptions.LogExcep(ex);

                //Catch request timeout exceptions
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Access Timeout";
                    message = "Access timeout, try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }
                await ModifyHeader(context, title, message, statusCode);

                //If request is unauthorized. 401 statuscode
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized";
                    message = "You are not authorized to access";
                    statusCode = StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //If request is forbiden. 403 statusCode.
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbiden";
                    message = "Access forbiden";
                    statusCode = StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }),CancellationToken.None);
            return;
        }
    }
}
