using Infinite.Core.Extensions;
using Infinite.Core.Interfaces.Services;
using Infinite.Core.Interfaces.Services.Identity;
using Infinite.Core.Services.Identity;
using Infinite.Server.Internal.Extensions;
using Infinite.Server.Internal.Middlewares;
using Infinite.Server.Internal.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddCurrentUserService();
builder.Services.AddDatabase(builder.Configuration, "DefaultConnection");
builder.Services.AddIdentity();
builder.Services.AddTransient<ITokenService, InternalTokenService>();
builder.Services.EnableAuthentication(builder.Configuration);
builder.Services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.EnableSwagger();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapFallbackToFile("_Internal");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Novel Infinite - Internal");
    options.DisplayRequestDuration();
    options.RoutePrefix = "swagger";
});

app.Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseSeeder>().Initialize();

app.Run();