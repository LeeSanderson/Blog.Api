using System.Text.Json;
using System.Text.Json.Serialization;
using Blog.Api.Core.Interfaces;
using Blog.Api.Core.Services;
using Blog.Api.Core.Validators;
using Blog.Api.Functions.Extensions;
using Blog.Api.Infrastructure.Repositories;
using FluentValidation;
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
    .AddSingleton<IBlogPostRepository, InMemoryBlogPostRepository>()
    .AddSingleton<BlogPostValidator>()
    .AddSingleton<BlogPostService>()
    .AddOpenApiDocumentation();

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

app.Run();
