# GitHub Copilot Instructions for RepoGovernance

This document contains coding standards and guidelines for the RepoGovernance project. Please follow these guidelines when generating code suggestions.

## C# Coding Standards

### Variable Declarations
- **NEVER use the `var` keyword** - Always use explicit types for variable declarations
- ✅ Good: `List<UserOwnerRepo> results = new List<UserOwnerRepo>();`
- ❌ Bad: `var results = new List<UserOwnerRepo>();`
- ✅ Good: `string connectionString = configuration.GetConnectionString("DefaultConnection");`
- ❌ Bad: `var connectionString = configuration.GetConnectionString("DefaultConnection");`

### Naming Conventions
- Use PascalCase for public properties, methods, and classes
- Use camelCase for private fields and local variables
- Use descriptive names that clearly indicate the purpose
- Prefix private fields with underscore: `_privateField`

### Code Structure
- Always include proper using statements at the top of files
- Use meaningful namespace organization
- Keep methods focused and single-purpose
- Prefer composition over inheritance where appropriate

### Testing Requirements
- **ALL new code must include comprehensive unit tests**
- Test classes should follow the naming pattern: `{ClassUnderTest}Tests.cs`
- Test methods should follow the pattern: `{MethodUnderTest}_{Scenario}_{ExpectedResult}`
- Include tests for:
  - Happy path scenarios
  - Edge cases (null values, empty strings, boundary conditions)
  - Error handling scenarios
  - All public properties and methods
- Aim for high code coverage (>80% line coverage)
- Place test files in the appropriate subdirectory within `src/RepoGovernance.Tests/`

### Exception Handling
- Always use specific exception types
- Include meaningful error messages
- Handle exceptions at the appropriate level
- Use `ArgumentException` for invalid parameters
- Use `InvalidOperationException` for invalid state

### Async/Await
- Use async/await properly for asynchronous operations
- Always use `ConfigureAwait(false)` for library code
- Return `Task<T>` or `Task` for async methods
- Use proper naming with `Async` suffix for async methods

### Documentation
- **Update README.md** when adding new features or significant changes
- Include XML documentation comments for public APIs
- Document complex business logic with inline comments
- Keep documentation up-to-date with code changes

### Performance Guidelines
- Use `StringBuilder` for multiple string concatenations
- Prefer `List<T>` over `ArrayList`
- Use LINQ judiciously - consider performance implications
- Implement `IDisposable` properly for resource management

### Security Considerations
- Never hardcode secrets or connection strings
- Use configuration providers for sensitive data
- Validate all user inputs
- Use parameterized queries for database operations

### File Organization
- Group related functionality in appropriate namespaces
- Keep file size reasonable (prefer multiple smaller files)
- Use consistent file naming conventions
- Place models in the `Models` folder
- Place data access in appropriate DA classes
- Place API access classes in the `APIAccess` folder
- Place utility classes in the `Helpers` folder

### Code Quality
- Follow SOLID principles
- Write self-documenting code
- Avoid deep nesting (use early returns)
- Keep cyclomatic complexity low
- Use meaningful variable and method names
- Remove unused using statements and code

### Dependencies
- Minimize external dependencies
- Use well-established, maintained packages
- Keep package versions up to date
- Document any new dependencies in the README

## Examples

### Correct Variable Declaration
```csharp
// ✅ Good - Explicit types
public async Task<List<UserOwnerRepo>> GetReposAsync(string connectionString, string user)
{
    List<UserOwnerRepo> results = new List<UserOwnerRepo>();
    AzureTableStorageDA storage = new AzureTableStorageDA();
    string processedUser = user.Trim().ToLower();
    
    return results;
}
```

### Incorrect Variable Declaration
```csharp
// ❌ Bad - Using var keyword
public async Task<List<UserOwnerRepo>> GetReposAsync(string connectionString, string user)
{
    var results = new List<UserOwnerRepo>();  // Should be explicit type
    var storage = new AzureTableStorageDA();  // Should be explicit type
    var processedUser = user.Trim().ToLower(); // Should be explicit type
    
    return results;
}
```

### Test Example
```csharp
[TestMethod]
public void GetColorFromStatus_ValidStatus_ReturnsExpectedColor()
{
    // Arrange
    string status = "success";
    string expectedColor = "green";
    
    // Act
    string actualColor = DotNetRepoScanner.GetColorFromStatus(status);
    
    // Assert
    Assert.AreEqual(expectedColor, actualColor);
}
```

Please ensure all generated code follows these guidelines to maintain consistency and quality across the RepoGovernance codebase.