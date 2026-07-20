namespace Blog.Api.Core.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

public static class ApiResponse
{
    public static ApiResponse<T> SuccessResponse<T>(T data) =>
        SuccessResponse(data, "Operation completed successfully");

    public static ApiResponse<T> SuccessResponse<T>(T data, string message) =>
        new()
        {
            Success = true,
            Message = message,
            Data = data,
        };

    public static ApiResponse<T> ErrorResponse<T>(IEnumerable<string> errors) =>
        ErrorResponse<T>(errors, "Operation failed");

    public static ApiResponse<T> ErrorResponse<T>(IEnumerable<string> errors, string message) =>
        new()
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors,
        };

    public static ApiResponse<T> NotFoundResponse<T>() =>
        NotFoundResponse<T>("Resource not found");

    public static ApiResponse<T> NotFoundResponse<T>(string message) =>
        new()
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = new[] { message },
        };
}
