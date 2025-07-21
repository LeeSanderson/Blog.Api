using Blog.Api.Core.Models;
using Blog.Api.Domain.Models;

namespace Blog.Api.Core.Interfaces;

public interface IBlogPostRepository
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    
    Task<PagedList<BlogPost>> GetAllAsync(PaginationParameters parameters);
    
    Task<IEnumerable<BlogPost>> SearchAsync(string searchTerm);
    
    Task<PagedList<BlogPost>> SearchAsync(string searchTerm, PaginationParameters parameters);
    
    Task<BlogPost?> GetByIdAsync(int id);
    
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    
    Task<BlogPost?> UpdateAsync(int id, BlogPost blogPost);
    
    Task<bool> DeleteAsync(int id);
}
