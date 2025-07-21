using Blog.Api.Core.Interfaces;
using Blog.Api.Core.Models;
using Blog.Api.Core.Validators;
using Blog.Api.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Blog.Api.Core.Services;

public class BlogPostService
{
    private readonly IBlogPostRepository _repository;
    private readonly ILogger<BlogPostService> _logger;
    private readonly IBlogPostValidator _validator;

    public BlogPostService(IBlogPostRepository repository, ILogger<BlogPostService> logger, IBlogPostValidator? validator = null)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator ?? new BlogPostValidator();
    }

    public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
    {
        _logger.LogInformation("Getting all blog posts");
        return await _repository.GetAllAsync();
    }

    public async Task<PagedList<BlogPost>> GetBlogPostsAsync(PaginationParameters parameters)
    {
        _logger.LogInformation("Getting paged blog posts - Page: {PageNumber}, Size: {PageSize}", 
            parameters.PageNumber, parameters.PageSize);
        return await _repository.GetAllAsync(parameters);
    }

    public async Task<IEnumerable<BlogPost>> SearchBlogPostsAsync(string searchTerm)
    {
        _logger.LogInformation("Searching blog posts with term: {SearchTerm}", searchTerm);
        return await _repository.SearchAsync(searchTerm);
    }

    public async Task<PagedList<BlogPost>> SearchBlogPostsAsync(string searchTerm, PaginationParameters parameters)
    {
        _logger.LogInformation("Searching paged blog posts with term: {SearchTerm} - Page: {PageNumber}, Size: {PageSize}", 
            searchTerm, parameters.PageNumber, parameters.PageSize);
        return await _repository.SearchAsync(searchTerm, parameters);
    }

    public async Task<BlogPost?> GetBlogPostByIdAsync(int id)
    {
        _logger.LogInformation("Getting blog post with ID: {Id}", id);
        return await _repository.GetByIdAsync(id);
    }

    public async Task<(BlogPost? blogPost, IEnumerable<string> errors)> CreateBlogPostAsync(BlogPost blogPost)
    {
        _logger.LogInformation("Creating new blog post with title: {Title}", blogPost.Title);
        
        // Validate the blog post
        var validationResult = await _validator.ValidateAsync(blogPost);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Blog post validation failed");
            return (null, validationResult.Errors.Select(e => e.ErrorMessage));
        }
        
        var createdPost = await _repository.CreateAsync(blogPost);
        return (createdPost, Array.Empty<string>());
    }

    public async Task<(BlogPost? blogPost, IEnumerable<string> errors)> UpdateBlogPostAsync(int id, BlogPost blogPost)
    {
        _logger.LogInformation("Updating blog post with ID: {Id}", id);
        
        // Validate the blog post
        var validationResult = await _validator.ValidateAsync(blogPost);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Blog post validation failed for update");
            return (null, validationResult.Errors.Select(e => e.ErrorMessage));
        }
        
        var updatedPost = await _repository.UpdateAsync(id, blogPost);
        return (updatedPost, updatedPost == null 
            ? new[] { $"Blog post with ID {id} not found" } 
            : Array.Empty<string>());
    }

    public async Task<bool> DeleteBlogPostAsync(int id)
    {
        _logger.LogInformation("Deleting blog post with ID: {Id}", id);
        return await _repository.DeleteAsync(id);
    }
}
