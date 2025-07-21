using Blog.Api.Core.Interfaces;
using Blog.Api.Core.Models;
using Blog.Api.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Blog.Api.Infrastructure.Repositories;

public class InMemoryBlogPostRepository : IBlogPostRepository
{
    private readonly ILogger<InMemoryBlogPostRepository> _logger;
    private readonly List<BlogPost> _blogPosts = new();
    private int _nextId = 1;

    public InMemoryBlogPostRepository(ILogger<InMemoryBlogPostRepository> logger)
    {
        _logger = logger;
        
        // Add some sample data
        _blogPosts.Add(new BlogPost
        {
            Id = _nextId++,
            Title = "Getting Started with Azure Functions",
            Content = "Azure Functions is a serverless compute service that enables you to run code on-demand without having to explicitly provision or manage infrastructure.",
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            Author = "John Doe",
            Tags = new List<string> { "Azure", "Serverless", "Cloud" }
        });
        
        _blogPosts.Add(new BlogPost
        {
            Id = _nextId++,
            Title = "Clean Architecture in .NET",
            Content = "Clean Architecture is a software design philosophy that separates the elements of a design into ring levels.",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            Author = "Jane Smith",
            Tags = new List<string> { ".NET", "Architecture", "Best Practices" }
        });
        
        // Add more sample data
        _blogPosts.Add(new BlogPost
        {
            Id = _nextId++,
            Title = "Dependency Injection in .NET",
            Content = "Dependency injection is a design pattern that allows the creation of dependent objects outside of a class and provides those objects to a class through different ways.",
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            Author = "Bob Johnson",
            Tags = new List<string> { ".NET", "Design Patterns", "Best Practices" }
        });
        
        _blogPosts.Add(new BlogPost
        {
            Id = _nextId++,
            Title = "Working with Azure Cosmos DB",
            Content = "Azure Cosmos DB is a fully managed NoSQL database service for modern app development. It offers multi-master replication, guaranteed single-digit millisecond response times, and 99.999-percent availability.",
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            Author = "Alice Williams",
            Tags = new List<string> { "Azure", "Database", "NoSQL", "Cosmos DB" }
        });
    }

    public Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        _logger.LogInformation("Getting all {Count} blog posts from repository", _blogPosts.Count);
        return Task.FromResult<IEnumerable<BlogPost>>(_blogPosts);
    }
    
    public Task<PagedList<BlogPost>> GetAllAsync(PaginationParameters parameters)
    {
        _logger.LogInformation("Getting paged blog posts - Page: {PageNumber}, Size: {PageSize}", 
            parameters.PageNumber, parameters.PageSize);
            
        var pagedList = PagedList<BlogPost>.Create(
            _blogPosts.OrderByDescending(p => p.CreatedAt), 
            parameters.PageNumber, 
            parameters.PageSize);
            
        return Task.FromResult(pagedList);
    }
    
    public Task<IEnumerable<BlogPost>> SearchAsync(string searchTerm)
    {
        _logger.LogInformation("Searching blog posts with term: {SearchTerm}", searchTerm);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetAllAsync();
        }
        
        searchTerm = searchTerm.ToLowerInvariant();
        
        var results = _blogPosts.Where(p => 
            p.Title.ToLowerInvariant().Contains(searchTerm) || 
            p.Content.ToLowerInvariant().Contains(searchTerm) ||
            p.Author.ToLowerInvariant().Contains(searchTerm) ||
            p.Tags.Any(tag => tag.ToLowerInvariant().Contains(searchTerm))
        );
        
        return Task.FromResult(results);
    }
    
    public Task<PagedList<BlogPost>> SearchAsync(string searchTerm, PaginationParameters parameters)
    {
        _logger.LogInformation("Searching paged blog posts with term: {SearchTerm} - Page: {PageNumber}, Size: {PageSize}", 
            searchTerm, parameters.PageNumber, parameters.PageSize);
            
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetAllAsync(parameters);
        }
        
        searchTerm = searchTerm.ToLowerInvariant();
        
        var results = _blogPosts.Where(p => 
            p.Title.ToLowerInvariant().Contains(searchTerm) || 
            p.Content.ToLowerInvariant().Contains(searchTerm) ||
            p.Author.ToLowerInvariant().Contains(searchTerm) ||
            p.Tags.Any(tag => tag.ToLowerInvariant().Contains(searchTerm))
        ).OrderByDescending(p => p.CreatedAt);
        
        var pagedList = PagedList<BlogPost>.Create(results, parameters.PageNumber, parameters.PageSize);
        
        return Task.FromResult(pagedList);
    }

    public Task<BlogPost?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting blog post with ID {Id} from repository", id);
        var blogPost = _blogPosts.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(blogPost);
    }

    public Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        var newBlogPost = blogPost with { Id = _nextId++, CreatedAt = DateTime.UtcNow };
        _blogPosts.Add(newBlogPost);
        _logger.LogInformation("Created new blog post with ID {Id}", newBlogPost.Id);
        return Task.FromResult(newBlogPost);
    }

    public Task<BlogPost?> UpdateAsync(int id, BlogPost blogPost)
    {
        _logger.LogInformation("Updating blog post with ID {Id}", id);
        var existingIndex = _blogPosts.FindIndex(p => p.Id == id);
        if (existingIndex == -1)
        {
            _logger.LogWarning("Blog post with ID {Id} not found for update", id);
            return Task.FromResult<BlogPost?>(null);
        }

        var updatedPost = blogPost with
        {
            Id = id,
            CreatedAt = _blogPosts[existingIndex].CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        _blogPosts[existingIndex] = updatedPost;
        return Task.FromResult<BlogPost?>(updatedPost);
    }

    public Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting blog post with ID {Id}", id);
        var removed = _blogPosts.RemoveAll(p => p.Id == id) > 0;
        if (!removed)
        {
            _logger.LogWarning("Blog post with ID {Id} not found for deletion", id);
        }
        return Task.FromResult(removed);
    }
}
