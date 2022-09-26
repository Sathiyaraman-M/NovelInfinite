using Infinite.Core.Features;
using Infinite.Core.Interfaces.Repositories;
using Infinite.Core.Interfaces.Services;
using Infinite.Core.Persistence;
using Infinite.Core.Repositories;
using Infinite.Core.Services;
using Infinite.Server.Middlewares;
using Infinite.Shared.Configurations;
using Infinite.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Infinite.Server;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MailConfiguration>(builder.Configuration.GetSection(nameof(MailConfiguration)));
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddIdentityServer()
            .AddApiAuthorization<AppUser, AppDbContext>();
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
        builder.Services.AddAuthorization();
        builder.Services.ConfigureCoreServices();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.EnableSwagger();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
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

    private static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        services.AddTransient<IManageAccountService, ManageAccountService>();
    }

    private static void EnableSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic) continue;
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(baseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            }
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Novel Infinite",
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                },
            });
        });
    }
}