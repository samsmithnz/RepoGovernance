# NuGet Package Scanning - Local Implementation

This document describes the new local NuGet package scanning capabilities that were moved from the GitHub Action workflow into the application code.

## Problem Solved

Previously, the logic for scanning .NET projects for deprecated, outdated, and vulnerable NuGet packages was embedded in the GitHub Action workflow (`nightlyprocess.yml`). This meant:
- The application could only process NuGet scan results, not generate them
- The scanning logic was duplicated in PowerShell scripts
- Local development and testing was difficult
- The application was dependent on GitHub Actions for this functionality

## Solution

The NuGet package scanning logic has been moved into the application code with these new capabilities:

### New Methods in `DotNetPackages` Class

```csharp
// Execute individual dotnet list package commands
string GetNugetPackagesDeprecatedJson(string path)
string GetNugetPackagesOutdatedJson(string path) 
string GetNugetPackagesVulnerableJson(string path)

// Support methods
string RestorePackages(string path)

// Comprehensive scanning (equivalent to GitHub Action workflow)
NugetScanResults ScanAllPackages(string path)
```

### New Method in `SummaryItemsDA` Class

```csharp
// Update summary with local NuGet scanning
Task<int> UpdateSummaryItemWithLocalNuGetScan(
    string clientId, string secret, string connectionString, 
    string devOpsServiceURL, string user, string owner, string repo,
    string azureTenantId, string azureClientId, string azureClientSecret,
    AzureDeployment azureDeployment = null, 
    string localRepositoryPath = null)
```

### New Model

```csharp
public class NugetScanResults
{
    public string DeprecatedJson { get; set; }
    public string OutdatedJson { get; set; }
    public string VulnerableJson { get; set; }
}
```

## Usage Examples

### Basic Scanning

```csharp
DotNetPackages scanner = new();

// Scan for all package types
NugetScanResults results = scanner.ScanAllPackages("/path/to/solution/directory");

// Individual scans
string deprecated = scanner.GetNugetPackagesDeprecatedJson("/path/to/solution");
string outdated = scanner.GetNugetPackagesOutdatedJson("/path/to/solution");
string vulnerable = scanner.GetNugetPackagesVulnerableJson("/path/to/solution");
```

### Integration with Summary Updates

```csharp
// Update summary item with local scanning
int updated = await SummaryItemsDA.UpdateSummaryItemWithLocalNuGetScan(
    githubClientId, githubSecret, connectionString, devOpsUrl,
    "user", "owner", "repo", azureTenantId, azureClientId, azureSecret,
    null, // azureDeployment
    "/path/to/local/repo/checkout");
```

## Migration from GitHub Action

The GitHub Action workflow previously contained this PowerShell logic:

```powershell
# Old GitHub Action approach
dotnet restore
$resultDeprecated = dotnet list package --deprecated --format json
$resultOutDated = dotnet list package --outdated --format json  
$resultVulnerable = dotnet list package --vulnerable --format json

# Process and send to API...
```

This is now replaced with:

```csharp
// New application code approach
DotNetPackages scanner = new();
NugetScanResults results = scanner.ScanAllPackages(solutionPath);

// Results are automatically available in:
// - results.DeprecatedJson
// - results.OutdatedJson  
// - results.VulnerableJson
```

## Benefits

1. **Self-contained**: Application can perform NuGet scanning without external dependencies
2. **Testable**: Logic can be unit tested and debugged locally
3. **Reusable**: Scanning functionality available to any part of the application
4. **Maintainable**: No more PowerShell script duplication
5. **Cross-platform**: Works on Windows, Linux, and macOS

## Backward Compatibility

The existing `UpdateSummaryItem` method that accepts NuGet payload parameters still works unchanged. The new functionality is additive and doesn't break existing workflows.

## Testing

New unit tests have been added to validate:
- Individual scanning methods work correctly
- Comprehensive scanning produces expected results
- Error handling for invalid paths
- Integration with existing summary update workflow

See `DotNetPackagesTests.cs` for test examples.