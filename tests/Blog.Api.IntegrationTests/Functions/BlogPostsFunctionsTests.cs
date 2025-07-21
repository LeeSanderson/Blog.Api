using Blog.Api.Core.Interfaces;
using Blog.Api.Core.Services;
using Blog.Api.Domain.Models;
using Blog.Api.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Blog.Api.IntegrationTests.Functions;

public class BlogPostsFunctionsTests
{
    private readonly BlogPostService _blogPostService;
    private readonly IBlogPostRepository _repository;

    public BlogPostsFunctionsTests()
    {
        // Create a real repository and service with a null logger
        _repository = new InMemoryBlogPostRepository(NullLogger<InMemoryBlogPostRepository>.Instance);
        _blogPostService = new BlogPostService(_repository, NullLogger<BlogPostService>.Instance);
    }

    [Fact]
    public async Task GetAllBlogPosts_ShouldReturnPosts()
    {
        // Act
        var posts = await _blogPostService.GetAllBlogPostsAsync();

        // Assert
        posts.Should().NotBeNull();
        posts.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetBlogPostById_WithValidId_ShouldReturnPost()
    {
        // Arrange
        var newPost = new BlogPost 
        { 
            Title = "Test Post", 
            Content = "Test Content", 
            Author = "Integration Test" 
        };
        
        var (createdPost, errors) = await _blogPostService.CreateBlogPostAsync(newPost);
        
        // Ensure the post was created successfully
        createdPost.Should().NotBeNull();
        errors.Should().BeEmpty();
        
        // Act
        var retrievedPost = await _blogPostService.GetBlogPostByIdAsync(createdPost!.Id);

        // Assert
        retrievedPost.Should().NotBeNull();
        retrievedPost!.Id.Should().Be(createdPost.Id);
        retrievedPost.Title.Should().Be(newPost.Title);
        retrievedPost.Content.Should().Be(newPost.Content);
        retrievedPost.Author.Should().Be(newPost.Author);
    }
}
