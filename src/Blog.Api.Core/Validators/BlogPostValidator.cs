using Blog.Api.Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Blog.Api.Core.Validators;

public class BlogPostValidator : AbstractValidator<BlogPost>, IBlogPostValidator
{
    public BlogPostValidator()
    {
        RuleFor(post => post.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(post => post.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(post => post.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(50).WithMessage("Author name must not exceed 50 characters.");

        RuleFor(post => post.Tags)
            .Must(tags => tags == null || tags.Count <= 10)
            .WithMessage("A blog post can have at most 10 tags.");
    }
}
