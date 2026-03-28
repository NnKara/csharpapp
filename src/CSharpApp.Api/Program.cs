using CSharpApp.Api;
using CSharpApp.Api.Configuration;
using CSharpApp.Api.PerformanceMiddleware;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration(builder.Configuration);
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddSingleton<RequestPerformanceMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<RequestPerformanceMiddleware>();
app.UseGlobalExceptionHandling();


//app.UseHttpsRedirection();

app.RouteApiEndpoints();

app.Run();

