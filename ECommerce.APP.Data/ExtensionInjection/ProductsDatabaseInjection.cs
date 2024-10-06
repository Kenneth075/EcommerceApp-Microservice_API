using ECommerce.APP.Data.Repositories;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.Service.OrderServices;
using ECommerce.APP.Service.Repositories;
using ECommerce.APP.SharedLibrary.ServiceContainer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Retry;
using Polly;
using ECommerce.APP.SharedLibrary.Logs;

namespace ECommerce.APP.Data.ExtensionInjection
{
    public static class ProductsDatabaseInjection
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Database registration
            SharedServiceContainer.AddSharedService<AppDbContext>(services, configuration, configuration["MySerilog:FileName"]);

            //Register HttpClient service
            services.AddHttpClient<IOrderService, OrderServiceRepository>(options =>
            {
                options.BaseAddress = new Uri(configuration["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(1);
            });

            //Create Retry Strategy
            var reTryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    var message = $"OnRetry, Attemp: {args.AttemptNumber}, Outcome {args.Outcome}";
                    LogExceptions.LogToDebug(message);
                    LogExceptions.LogToConsole(message);
                    return ValueTask.CompletedTask;
                }
            };

            //Use Retry Strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(reTryStrategy);
            });

            //Register repository services
            services.AddScoped<IProductInterface, ProductRepository>();  

            services.AddScoped<IOrder, OrderRepository>();
            services.AddScoped<IUserInterface, AppUserRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrasturePolicy(this IApplicationBuilder app)
        {
            app.UseSharedServicePolicy();

            return app;
        }
        
    }
}
