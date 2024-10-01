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
        public static IServiceCollection AddProductsDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Database registration
            SharedServiceContainer.AddSharedService<AppDbContext>(services, configuration, configuration["MySerilog:FileName"]);

            //Register Product repository
            services.AddScoped<IProductInterface, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UserProductInfrasturePolicy(this IApplicationBuilder app)
        {
            app.UseSharedServicePolicy();

            return app;
        }
        
    }
}
