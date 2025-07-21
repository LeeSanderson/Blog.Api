# Blog API - Azure Functions

This is a boilerplate Azure Functions project for a Blog API built with best practices.

## Project Structure

The solution follows Clean Architecture principles with the following projects:

- **Blog.Api.Functions**: Azure Functions project that exposes the HTTP endpoints
- **Blog.Api.Core**: Contains business logic and interfaces
- **Blog.Api.Domain**: Contains domain models and entities
- **Blog.Api.Infrastructure**: Contains implementation of repositories and external services
- **Blog.Api.UnitTests**: Unit tests for the solution
- **Blog.Api.IntegrationTests**: Integration tests for the solution

## Technologies

- .NET 8.0
- Azure Functions
- xUnit for testing
- FluentAssertions for test assertions
- Moq for mocking
- StyleCop for code analysis
- EditorConfig for code style enforcement

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Azure Functions Core Tools
- Visual Studio 2022 or Visual Studio Code

### Building the Solution

```powershell
dotnet build
```

### Running the Tests

```powershell
dotnet test
```

### Running the Azure Functions Locally

```powershell
cd src/Blog.Api.Functions
func start
```

## Code Quality

This solution uses:

- StyleCop Analyzers for code style enforcement
- .NET Analyzers for code quality analysis
- EditorConfig for consistent code style
- Directory.Build.props for common project settings

## Testing

The solution includes:

- Unit tests using xUnit, FluentAssertions, and Moq
- Integration tests for Functions
- Code coverage reporting with coverlet

## API Endpoints

| Method | Endpoint      | Description                |
|--------|---------------|----------------------------|
| GET    | /api/posts    | Get all blog posts        |
| GET    | /api/posts/{id} | Get blog post by ID      |
| POST   | /api/posts    | Create a new blog post    |
| PUT    | /api/posts/{id} | Update an existing blog post |
| DELETE | /api/posts/{id} | Delete a blog post      |
