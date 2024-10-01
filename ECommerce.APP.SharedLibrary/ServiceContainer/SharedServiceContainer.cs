using ECommerce.APP.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ECommerce.APP.SharedLibrary.ServiceContainer
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TContext>(this IServiceCollection services,
            IConfiguration configuration, string fileName) where TContext : DbContext
        {
            //Add generic dbcontext
            services.AddDbContext<TContext>(option => option.UseSqlServer(configuration.GetConnectionString("DbCon"),
                sqlOpt => sqlOpt.EnableRetryOnFailure()));

            //Configure serilog logging.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File($"{fileName}-.text",
                 restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                 rollingInterval: RollingInterval.Day)
                 .CreateLogger();

            //Register Jwt
            services.AddJwtAuthenticationScheme(configuration);

            return services;
        }

        public static IApplicationBuilder UseSharedServicePolicy(this IApplicationBuilder app)
        {
            //Register middleware to block all outside middleware call.
            app.UseMiddleware<ApiGatewayMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            return app;
        }
    }
}
