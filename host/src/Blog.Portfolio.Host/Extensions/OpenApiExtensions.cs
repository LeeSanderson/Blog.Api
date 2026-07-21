using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Blog.Portfolio.Host.Extensions;

internal static class OpenApiExtensions
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
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
                    Email = "support@blog.api.example",
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
#pragma warning disable S1075 // Static, well-known license URL, not an environment-specific endpoint
                    Url = new Uri("https://opensource.org/licenses/MIT"),
#pragma warning restore S1075
                },
            },
            OpenApiVersion = OpenApiVersionType.V3,
            IncludeRequestingHostName = true,
            ForceHttps = false,
            ForceHttp = false,
        });

        return services;
    }
}