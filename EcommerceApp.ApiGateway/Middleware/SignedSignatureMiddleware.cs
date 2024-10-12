namespace EcommerceApp.ApiGateway.Middleware
{
    public class SignedSignatureMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers["Api_Gateway"] = "Signed";
            await next(context);
        }
    }
}
