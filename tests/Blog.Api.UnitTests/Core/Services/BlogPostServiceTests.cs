using Blog.Api.Core.Interfaces;
using Blog.Api.Core.Services;
using Blog.Api.Core.Validators;
using Blog.Api.Domain.Models;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Blog.Api.UnitTests.Core.Services;

public class BlogPostServiceTests
{
    private readonly Mock<IBlogPostRepository> _mockRepository;
    private readonly Mock<ILogger<BlogPostService>> _mockLogger;
    private readonly Mock<IBlogPostValidator> _mockValidator;
    private readonly BlogPostService _service;

    public BlogPostServiceTests()
    {
        _mockRepository = new Mock<IBlogPostRepository>();
        _mockLogger = new Mock<ILogger<BlogPostService>>();
        _mockValidator = new Mock<IBlogPostValidator>();
        
        // Setup default validation behavior to return valid
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<BlogPost>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _service = new BlogPostService(_mockRepository.Object, _mockLogger.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task GetAllBlogPostsAsync_ShouldReturnAllPosts()
    {
        // Arrange
        var expectedPosts = new List<BlogPost>
        {
            new BlogPost { Id = 1, Title = "Test Post 1" },
            new BlogPost { Id = 2, Title = "Test Post 2" }
        };

        _mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedPosts);

        // Act
        var result = await _service.GetAllBlogPostsAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedPosts);
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetBlogPostByIdAsync_WithValidId_ShouldReturnPost()
    {
        // Arrange
        var postId = 1;
        var expectedPost = new BlogPost { Id = postId, Title = "Test Post" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(postId))
            .ReturnsAsync(expectedPost);

        // Act
        var result = await _service.GetBlogPostByIdAsync(postId);

        // Assert
        result.Should().BeEquivalentTo(expectedPost);
        _mockRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
    }

    [Fact]
    public async Task CreateBlogPostAsync_ShouldReturnCreatedPost()
    {
        // Arrange
        var postToCreate = new BlogPost { Title = "New Post", Content = "New Content" };
        var expectedPost = new BlogPost { Id = 1, Title = "New Post", Content = "New Content", CreatedAt = DateTime.UtcNow };

        _mockRepository.Setup(repo => repo.CreateAsync(postToCreate))
            .ReturnsAsync(expectedPost);

        // Act
        var (resultPost, errors) = await _service.CreateBlogPostAsync(postToCreate);

        // Assert
        resultPost.Should().BeEquivalentTo(expectedPost);
        errors.Should().BeEmpty();
        _mockRepository.Verify(repo => repo.CreateAsync(postToCreate), Times.Once);
    }

    [Fact]
    public async Task UpdateBlogPostAsync_WithValidId_ShouldReturnUpdatedPost()
    {
        // Arrange
        var postId = 1;
        var postToUpdate = new BlogPost { Title = "Updated Post", Content = "Updated Content" };
        var expectedPost = new BlogPost { Id = postId, Title = "Updated Post", Content = "Updated Content", UpdatedAt = DateTime.UtcNow };

        _mockRepository.Setup(repo => repo.UpdateAsync(postId, postToUpdate))
            .ReturnsAsync(expectedPost);

        // Act
        var (resultPost, errors) = await _service.UpdateBlogPostAsync(postId, postToUpdate);

        // Assert
        resultPost.Should().BeEquivalentTo(expectedPost);
        errors.Should().BeEmpty();
        _mockRepository.Verify(repo => repo.UpdateAsync(postId, postToUpdate), Times.Once);
    }

    [Fact]
    public async Task DeleteBlogPostAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var postId = 1;

        _mockRepository.Setup(repo => repo.DeleteAsync(postId))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteBlogPostAsync(postId);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(repo => repo.DeleteAsync(postId), Times.Once);
    }
}
