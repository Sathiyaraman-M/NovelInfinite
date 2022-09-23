using Infinite.Core.Extensions;
using Infinite.Core.Persistence;
using Infinite.Server.Extensions;
using Infinite.Server.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Server;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.EnableSwagger();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseBlazorFrameworkFiles();
        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapRazorPages();
        app.MapFallbackToFile("index.html");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Novel Infinite");
            options.DisplayRequestDuration();
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}