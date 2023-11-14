using AWSGeocodeAPI.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace AWSGeocodeAPI;

public record ExceptionHandlerDTO(string Message, string? InnerMessage);
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddAWSDynamoDBDistributedCache(options => 
        {
            options.CreateTableIfNotExists = true;
            options.TableName = "CacheTable";
            options.PartitionKeyName = "id";
            options.TTLAttributeName = "ttl";
        });
        services.AddTransient<SecretsManagerService>();
        services.AddHttpClient("GoogleMaps", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/geocode/json");
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(options => options.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                await context.Response.WriteAsJsonAsync(
                    new ExceptionHandlerDTO(exception.Message, exception.InnerException?.Message)
                );
            }));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}