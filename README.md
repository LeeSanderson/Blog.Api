# Blog API - Azure Functions

This is a boilerplate Azure Functions project for a Blog API built with best practices.

## Project Structure

The solution currently contains the following projects:

- **Blog.Api.Functions**: Azure Functions project that exposes the HTTP endpoints
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

None yet — the project has been stripped back to a bare Azure Functions boilerplate.
