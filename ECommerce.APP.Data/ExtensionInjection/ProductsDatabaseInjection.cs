using ECommerce.APP.Data.Repositories;
using ECommerce.APP.Service.Interfaces;
using ECommerce.APP.Service.Repositories;
using ECommerce.APP.SharedLibrary.ServiceContainer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.APP.Data.ExtensionInjection
{
    public static class ProductsDatabaseInjection
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Database registration
            SharedServiceContainer.AddSharedService<AppDbContext>(services, configuration, configuration["MySerilog:FileName"]);

            //Register repository services
            services.AddScoped<IProductInterface, ProductRepository>();  

            services.AddScoped<IOrder, OrderRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrasturePolicy(this IApplicationBuilder app)
        {
            app.UseSharedServicePolicy();

            return app;
        }
        
    }
}
