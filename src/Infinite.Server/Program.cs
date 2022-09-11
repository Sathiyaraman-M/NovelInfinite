using Infinite.Core.Extensions;
using Infinite.Core.Interfaces.Services;
using Infinite.Server.Extensions;
using Infinite.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddCurrentUserService();
builder.Services.AddDatabase(builder.Configuration, "DefaultConnection");
builder.Services.AddIdentity();
builder.Services.EnableAuthentication(builder.Configuration);
builder.Services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapFallbackToFile("index.html");
});

app.Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseSeeder>().Initialize();

app.Run();
