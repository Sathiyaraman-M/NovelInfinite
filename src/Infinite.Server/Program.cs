using Infinite.Server;

var app = WebApplication.CreateBuilder(args).ConfigureServices().ConfigurePipeline();
app.Run();