namespace Blog.Api.Domain.Models;

public record BlogPost
{
    public int Id { get; init; }
    
    public string Title { get; init; } = string.Empty;
    
    public string Content { get; init; } = string.Empty;
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; }
    
    public string Author { get; init; } = string.Empty;
    
    public ICollection<string> Tags { get; init; } = new List<string>();
}
