using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Blog.Api.Functions.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        // Configure OpenAPI
        services.AddSingleton(_ => new OpenApiConfigurationOptions
        {
            Info = new OpenApiInfo
            {
                Version = "v1",
                Title = "Blog API",
                Description = "A simple blog API using Azure Functions with Clean Architecture",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@blog.api.example"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            },
            OpenApiVersion = OpenApiVersionType.V3,
            IncludeRequestingHostName = true,
            ForceHttps = false,
            ForceHttp = false
        });

        return services;
    }
}
