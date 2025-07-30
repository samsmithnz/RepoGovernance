using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models.NuGetPackages;
using System;
using System.IO;

namespace RepoGovernance.Examples
{
    /// <summary>
    /// Example demonstrating how to use the new local NuGet scanning capabilities
    /// This replaces the logic previously only available in the GitHub Action workflow
    /// </summary>
    public class LocalNuGetScanningExample
    {
        /// <summary>
        /// Example showing how to perform local NuGet package scanning
        /// This demonstrates the functionality that was previously only in nightlyprocess.yml
        /// </summary>
        /// <param name="repositoryPath">Path to a local .NET repository</param>
        public static void PerformLocalNuGetScan(string repositoryPath)
        {
            if (!Directory.Exists(repositoryPath))
            {
                Console.WriteLine($"Repository path does not exist: {repositoryPath}");
                return;
            }

            Console.WriteLine($"Performing NuGet package scan on: {repositoryPath}");
            
            // Find .NET solution files
            string[] solutionFiles = Directory.GetFiles(repositoryPath, "*.sln", SearchOption.AllDirectories);
            if (solutionFiles.Length == 0)
            {
                Console.WriteLine("No .NET solution files found.");
                return;
            }

            // Use the directory containing the first solution file
            string solutionDir = Path.GetDirectoryName(solutionFiles[0]) ?? repositoryPath;
            Console.WriteLine($"Using solution directory: {solutionDir}");

            try
            {
                DotNetPackages dotNetPackages = new();
                
                // Perform comprehensive scan (equivalent to GitHub Action workflow)
                Console.WriteLine("Starting comprehensive NuGet package scan...");
                NugetScanResults results = dotNetPackages.ScanAllPackages(solutionDir);

                // Display results summary
                Console.WriteLine("\n=== SCAN RESULTS ===");
                Console.WriteLine($"Deprecated packages JSON length: {results.DeprecatedJson.Length} characters");
                Console.WriteLine($"Outdated packages JSON length: {results.OutdatedJson.Length} characters");
                Console.WriteLine($"Vulnerable packages JSON length: {results.VulnerableJson.Length} characters");

                // Parse and count packages (similar to GitHub Action logic)
                var deprecatedPackages = dotNetPackages.GetNugetPackagesDeprecated(results.DeprecatedJson);
                var outdatedPackages = dotNetPackages.GetNugetPackagesOutdated(results.OutdatedJson);
                var vulnerablePackages = dotNetPackages.GetNugetPackagesVulnerable(results.VulnerableJson);

                Console.WriteLine($"\nPackage counts:");
                Console.WriteLine($"  Deprecated: {deprecatedPackages.Count}");
                Console.WriteLine($"  Outdated: {outdatedPackages.Count}");
                Console.WriteLine($"  Vulnerable: {vulnerablePackages.Count}");

                // Show sample data (first few characters of JSON)
                Console.WriteLine($"\nSample JSON outputs:");
                Console.WriteLine($"Deprecated: {results.DeprecatedJson.Substring(0, Math.Min(200, results.DeprecatedJson.Length))}...");
                Console.WriteLine($"Outdated: {results.OutdatedJson.Substring(0, Math.Min(200, results.OutdatedJson.Length))}...");
                Console.WriteLine($"Vulnerable: {results.VulnerableJson.Substring(0, Math.Min(200, results.VulnerableJson.Length))}...");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during NuGet scan: {ex.Message}");
            }
        }

        /// <summary>
        /// Example showing individual scanning methods
        /// </summary>
        /// <param name="solutionDirectory">Path to directory containing .NET solution</param>
        public static void PerformIndividualScans(string solutionDirectory)
        {
            if (!Directory.Exists(solutionDirectory))
            {
                Console.WriteLine($"Solution directory does not exist: {solutionDirectory}");
                return;
            }

            Console.WriteLine($"Performing individual NuGet scans on: {solutionDirectory}");

            DotNetPackages dotNetPackages = new();

            try
            {
                // Restore packages first (equivalent to 'dotnet restore' in GitHub Action)
                Console.WriteLine("Restoring packages...");
                string restoreResult = dotNetPackages.RestorePackages(solutionDirectory);
                Console.WriteLine($"Restore completed. Output length: {restoreResult.Length} characters");

                // Individual scans (equivalent to individual dotnet list package commands in GitHub Action)
                Console.WriteLine("\nRunning individual scans...");
                
                string deprecatedJson = dotNetPackages.GetNugetPackagesDeprecatedJson(solutionDirectory);
                Console.WriteLine($"Deprecated scan completed: {deprecatedJson.Length} characters");

                string outdatedJson = dotNetPackages.GetNugetPackagesOutdatedJson(solutionDirectory);
                Console.WriteLine($"Outdated scan completed: {outdatedJson.Length} characters");

                string vulnerableJson = dotNetPackages.GetNugetPackagesVulnerableJson(solutionDirectory);
                Console.WriteLine($"Vulnerable scan completed: {vulnerableJson.Length} characters");

                Console.WriteLine("\nAll individual scans completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during individual scans: {ex.Message}");
            }
        }
    }
}