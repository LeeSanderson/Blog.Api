using System.Text.Json;
using System.Text.Json.Serialization;
using Blog.Portfolio.Host.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Register application services
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddOpenApiDocumentation()
    .AddCors();

// Configure JSON serialization
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// OpenAPI endpoints are automatically exposed in isolated Azure Functions
// at /api/swagger/ui and /api/openapi/{version}

await app.RunAsync();
