using Blog.Api.Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Blog.Api.Core.Validators;

public interface IBlogPostValidator
{
    Task<ValidationResult> ValidateAsync(BlogPost blogPost, CancellationToken cancellationToken = default);
}
