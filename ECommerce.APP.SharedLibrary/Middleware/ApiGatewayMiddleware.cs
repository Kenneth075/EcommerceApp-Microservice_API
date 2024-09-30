using Microsoft.AspNetCore.Http;

namespace ECommerce.APP.SharedLibrary.Middleware
{
    public class ApiGatewayMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            var signedHandeler = context.Request.Headers["Api_Gateway"];
            if(signedHandeler.FirstOrDefault() == null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, the service is unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
