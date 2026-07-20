namespace Blog.Api.Domain.Models;

public record BlogPost
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }

    public string Author { get; init; } = string.Empty;

    public ICollection<string> Tags { get; init; } = new List<string>();
}