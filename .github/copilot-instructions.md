# GitHub Copilot Instructions

These instructions define how GitHub Copilot should assist with this project. The goal is to ensure consistent, high-quality code generation aligned with our conventions, stack, and best practices.

## 🧠 Context

- **Project Type**: Web API / Console App / Blazor App / Microservice
- **Language**: C#
- **Framework / Libraries**: .NET 8+ / ASP.NET Core / Entity Framework Core / nUnit / NSubstitute
- **Architecture**: Clean Architecture / MVC / Onion

## 🔧 General Guidelines

- Make only high confidence suggestions when reviewing code changes.
- Always use the latest version C#, currently C# 13 features.
- Use C#-idiomatic patterns and follow .NET coding conventions.
- Use PascalCase for class names and methods; camelCase for local variables and parameters.
- Use named methods instead of anonymous lambdas in business logic.
- Use nullable reference types (`#nullable enable`) and async/await.
- Format using `dotnet format` or IDE auto-formatting tools.
- Prioritize readability, testability, and SOLID principles.

## 📁 File Structure

Use this structure as a guide when creating or updating files:

```text
src/
  Api/
  Aspire/
  Azure/
  Blazor/
  Console/
  Startup.business/
  Startup.Client/
  Startup.Common/
  Startup.Data/
  Startup.Tests/
  StartupExample/
  Web/
```

## 🧶 Patterns

### ✅ Patterns to Follow
- Use `.editorconfig` for consistent code style.
- Use Clean Architecture with layered separation.
- Use Dependency Injection for services and repositories.
- Use FluentValidation for input validation.
- Map DTOs to domain models using manual mapping extension classes and methods.
- Use ILogger<T> for structured logging.
- For APIs:
  - Use [ApiController], ActionResult<T>, and ProducesResponseType.
  - Handle errors using middleware and Problem Details.

### 🚫 Patterns to Avoid
- Don’t use static state or service locators.
- Avoid logic in controllers—delegate to services/handlers.
- Don’t hardcode config—use appsettings.json and IOptions.
- Don’t expose entities directly in API responses.
- Avoid fat controllers and God classes.

### Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## 🧪 Testing Guidelines
- Use nUnit for unit and integration testing.
- Use NSubstitute for mocking dependencies.
- Follow Arrange-Act-Assert pattern in tests.
- Validate edge cases and exceptions.
- Prefer TDD for critical business logic and application services.

## Running tests

(1) Build from the root with `dotnet build Ai.Coach.sln`.
(2) If that produces errors, fix those errors and build again. Repeat until the build is successful.
(3) Run tests with `dotnet test **/*.tests.csproj`.

## 🧩 Example Prompts
- `Copilot, generate an ASP.NET Core controller with CRUD endpoints for Product.`
- `Copilot, create an Entity Framework Core DbContext for a blog application.`
- `Copilot, write an nUnit test for the CalculateInvoiceTotal method.`

## 🔁 Iteration & Review
- Copilot output should be reviewed and modified before committing.
- If code isn’t following these instructions, regenerate with more context or split the task.
- Use /// XML documentation comments to clarify intent for Copilot and future devs.
- Use Rider or Visual Studio code inspections to catch violations early.

## 📚 References
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
- [Entity Framework Core Docs](https://learn.microsoft.com/en-us/ef/core/)
- [nUnit Documentation](https://nunit.org/)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Clean Architecture in .NET (by Jason Taylor)](https://github.com/jasontaylordev/CleanArchitecture)