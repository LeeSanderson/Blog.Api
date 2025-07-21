using System.Net;
using System.Web;
using Blog.Api.Core.Models;
using Blog.Api.Core.Services;
using Blog.Api.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Blog.Api.Functions;

public class BlogPostsFunctions
{
    private readonly ILogger _logger;
    private readonly BlogPostService _blogPostService;

    public BlogPostsFunctions(ILoggerFactory loggerFactory, BlogPostService blogPostService)
    {
        _logger = loggerFactory.CreateLogger<BlogPostsFunctions>();
        _blogPostService = blogPostService;
    }

    [Function("GetBlogPosts")]
    [OpenApiOperation(operationId: "GetBlogPosts", tags: new[] { "BlogPosts" }, Summary = "Get all blog posts", Description = "This retrieves all blog posts with optional pagination and search.")]
    [OpenApiParameter(name: "pageNumber", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "Page number for pagination")]
    [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "Number of items per page")]
    [OpenApiParameter(name: "search", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Optional search term to filter posts")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PagedList<BlogPost>), Summary = "Successful operation", Description = "Returns all blog posts")]
    public async Task<HttpResponseData> GetBlogPosts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequestData req)
    {
        _logger.LogInformation("Get all blog posts request received");
        
        // Parse query parameters for pagination
        var query = HttpUtility.ParseQueryString(req.Url.Query);
        
        // Check for pagination parameters
        if (int.TryParse(query["pageNumber"], out var pageNumber) && 
            int.TryParse(query["pageSize"], out var pageSize))
        {
            var parameters = new PaginationParameters 
            { 
                PageNumber = pageNumber > 0 ? pageNumber : 1, 
                PageSize = pageSize > 0 ? pageSize : 10 
            };
            
            // Check if search parameter is provided
            var searchTerm = query["search"];
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogInformation("Searching posts with term: {SearchTerm}, Page: {PageNumber}, Size: {PageSize}", 
                    searchTerm, parameters.PageNumber, parameters.PageSize);
                
                var pagedResults = await _blogPostService.SearchBlogPostsAsync(searchTerm, parameters);
                
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(pagedResults);
                return response;
            }
            
            _logger.LogInformation("Getting paged posts - Page: {PageNumber}, Size: {PageSize}", 
                parameters.PageNumber, parameters.PageSize);
            
            var pagedPosts = await _blogPostService.GetBlogPostsAsync(parameters);
            
            var pagedResponse = req.CreateResponse(HttpStatusCode.OK);
            await pagedResponse.WriteAsJsonAsync(pagedPosts);
            return pagedResponse;
        }
        
        // Check if search parameter is provided without pagination
        var searchTermOnly = query["search"];
        if (!string.IsNullOrWhiteSpace(searchTermOnly))
        {
            _logger.LogInformation("Searching posts with term: {SearchTerm}", searchTermOnly);
            var searchResults = await _blogPostService.SearchBlogPostsAsync(searchTermOnly);
            
            var searchResponse = req.CreateResponse(HttpStatusCode.OK);
            await searchResponse.WriteAsJsonAsync(searchResults);
            return searchResponse;
        }

        // Default: Get all posts without pagination
        var posts = await _blogPostService.GetAllBlogPostsAsync();
        
        var defaultResponse = req.CreateResponse(HttpStatusCode.OK);
        await defaultResponse.WriteAsJsonAsync(ApiResponse<IEnumerable<BlogPost>>.SuccessResponse(posts));
        return defaultResponse;
    }

    [Function("GetBlogPostById")]
    [OpenApiOperation(operationId: "GetBlogPostById", tags: new[] { "BlogPosts" }, Summary = "Get blog post by ID", Description = "This retrieves a specific blog post by ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Blog post ID")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BlogPost), Summary = "Successful operation", Description = "Returns the blog post with the specified ID")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Not found", Description = "Blog post not found")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Bad request", Description = "Invalid ID format")]
    public async Task<HttpResponseData> GetBlogPostById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation("Get blog post by ID request received for post ID: {PostId}", id);

        if (!int.TryParse(id, out var postId))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.ErrorResponse(new[] { "Invalid post ID format" }));
            return badResponse;
        }

        var post = await _blogPostService.GetBlogPostByIdAsync(postId);
        
        if (post == null)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            await notFoundResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.NotFoundResponse($"Blog post with ID {postId} not found"));
            return notFoundResponse;
        }
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(ApiResponse<BlogPost>.SuccessResponse(post));
        
        return response;
    }

    [Function("CreateBlogPost")]
    [OpenApiOperation(operationId: "CreateBlogPost", tags: new[] { "BlogPosts" }, Summary = "Create a new blog post", Description = "This creates a new blog post.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(BlogPost), Required = true, Description = "Blog post object to create")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Successful operation", Description = "Returns the created blog post")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Bad request", Description = "Invalid input")]
    public async Task<HttpResponseData> CreateBlogPost([HttpTrigger(AuthorizationLevel.Function, "post", Route = "posts")] HttpRequestData req)
    {
        _logger.LogInformation("Create blog post request received");

        var requestBody = await req.ReadFromJsonAsync<BlogPost>();
        
        if (requestBody == null)
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.ErrorResponse(new[] { "Invalid request body" }));
            return badResponse;
        }
        
        var (createdPost, errors) = await _blogPostService.CreateBlogPostAsync(requestBody);
        
        if (createdPost == null)
        {
            var validationResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await validationResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.ErrorResponse(errors, "Validation failed"));
            return validationResponse;
        }
        
        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(ApiResponse<BlogPost>.SuccessResponse(createdPost, "Blog post created successfully"));
        
        return response;
    }

    [Function("UpdateBlogPost")]
    [OpenApiOperation(operationId: "UpdateBlogPost", tags: new[] { "BlogPosts" }, Summary = "Update an existing blog post", Description = "This updates an existing blog post.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Blog post ID")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(BlogPost), Required = true, Description = "Updated blog post object")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Successful operation", Description = "Returns the updated blog post")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Not found", Description = "Blog post not found")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ApiResponse<BlogPost>), Summary = "Bad request", Description = "Invalid input")]
    public async Task<HttpResponseData> UpdateBlogPost([HttpTrigger(AuthorizationLevel.Function, "put", Route = "posts/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation("Update blog post request received for post ID: {PostId}", id);

        if (!int.TryParse(id, out var postId))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.ErrorResponse(new[] { "Invalid post ID format" }));
            return badResponse;
        }
        
        var requestBody = await req.ReadFromJsonAsync<BlogPost>();
        
        if (requestBody == null)
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.ErrorResponse(new[] { "Invalid request body" }));
            return badResponse;
        }
        
        var (updatedPost, errors) = await _blogPostService.UpdateBlogPostAsync(postId, requestBody);
        
        if (updatedPost == null)
        {
            // Check if it's a validation error or a not found error
            if (errors.Any(e => e.Contains("not found")))
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.NotFoundResponse($"Blog post with ID {postId} not found"));
                return notFoundResponse;
            }
            
            var validationResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await validationResponse.WriteAsJsonAsync(ApiResponse<BlogPost>.ErrorResponse(errors, "Validation failed"));
            return validationResponse;
        }
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(ApiResponse<BlogPost>.SuccessResponse(updatedPost, "Blog post updated successfully"));
        
        return response;
    }

    [Function("DeleteBlogPost")]
    [OpenApiOperation(operationId: "DeleteBlogPost", tags: new[] { "BlogPosts" }, Summary = "Delete a blog post", Description = "This deletes an existing blog post.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Blog post ID")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent, Summary = "Successful operation", Description = "Blog post deleted successfully")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(ApiResponse<object>), Summary = "Not found", Description = "Blog post not found")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ApiResponse<object>), Summary = "Bad request", Description = "Invalid ID format")]
    public async Task<HttpResponseData> DeleteBlogPost([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "posts/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation("Delete blog post request received for post ID: {PostId}", id);

        if (!int.TryParse(id, out var postId))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(ApiResponse<object>.ErrorResponse(new[] { "Invalid post ID format" }));
            return badResponse;
        }
        
        var deleted = await _blogPostService.DeleteBlogPostAsync(postId);
        
        if (!deleted)
        {
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            await notFoundResponse.WriteAsJsonAsync(ApiResponse<object>.NotFoundResponse($"Blog post with ID {postId} not found"));
            return notFoundResponse;
        }
        
        return req.CreateResponse(HttpStatusCode.NoContent);
    }
}
