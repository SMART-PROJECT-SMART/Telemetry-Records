using TelemetryRecords.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebApi();
builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddMongoDbServices();

WebApplication app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
