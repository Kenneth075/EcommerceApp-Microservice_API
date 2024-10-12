using EcommerceApp.ApiGateway.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.Middleware;
using ECommerce.APP.SharedLibrary.ServiceContainer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddJwtAuthenticationScheme(builder.Configuration);
JwtAuthenticationScheme.AddJwtAuthenticationScheme(builder.Services, builder.Configuration);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

var app = builder.Build();


app.UseCors();
app.UseMiddleware<SignedSignatureMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseOcelot().Wait();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
