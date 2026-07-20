using Blog.Api.Domain.Models;
using FluentValidation;

namespace Blog.Api.Core.Validators;

public class BlogPostValidator : AbstractValidator<BlogPost>, IBlogPostValidator
{
    private const int MaxTitleLength = 100;
    private const int MaxAuthorLength = 50;
    private const int MaxTagCount = 10;

    public BlogPostValidator()
    {
        RuleFor(post => post.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(MaxTitleLength).WithMessage($"Title must not exceed {MaxTitleLength} characters.");

        RuleFor(post => post.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(post => post.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(MaxAuthorLength).WithMessage($"Author name must not exceed {MaxAuthorLength} characters.");

        RuleFor(post => post.Tags)
            .Must(tags => tags == null || tags.Count <= MaxTagCount)
            .WithMessage($"A blog post can have at most {MaxTagCount} tags.");
    }
}