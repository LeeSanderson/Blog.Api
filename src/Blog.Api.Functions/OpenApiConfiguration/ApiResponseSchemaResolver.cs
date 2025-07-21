using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Blog.Api.Functions.OpenApiConfiguration;

public class ApiResponseSchemaExample<T>
{
    public T Data { get; set; }
}
