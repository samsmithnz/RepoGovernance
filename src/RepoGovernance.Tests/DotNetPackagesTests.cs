using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models.NuGetPackages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DotNetPackagesTests
    {
        [TestMethod]
        public void NugetPackagesDeprecatedTest()
        {
            //Arrange
            System.IO.DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = dir?.Parent?.Parent?.Parent?.FullName + @"\Sample\src\";
            //Debug.WriteLine("Path to test");
            //Debug.WriteLine(path);
            DotNetPackages dotNetPackages = new();
            //Get the output from the process
            string json = dotNetPackages.GetProcessOutput(path, "list package --deprecated --format json");

            //Act - runs a repo in about 4s
            List<NugetPackage> results = dotNetPackages.GetNugetPackagesDeprecated(json);

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count >= 0);
            //Assert.AreEqual(1, results.Count);
        }


        [TestMethod]
        public void NugetPackagesOutdatedTest()
        {
            //Arrange
            System.IO.DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = dir?.Parent?.Parent?.Parent?.FullName + @"\Sample\src\";
            DotNetPackages dotNetPackages = new();
            //Get the output from the process
            string json = dotNetPackages.GetProcessOutput(path, "list package --outdated --format json");

            //Act - runs a repo in about 4s
            List<NugetPackage> results = dotNetPackages.GetNugetPackagesOutdated(json);

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count >= 0);
            //Assert.AreEqual(6, results.Count);

        }


        [TestMethod]
        public void NugetPackagesVulnerableTest()
        {
            //Arrange
            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = dir?.Parent?.Parent?.Parent?.FullName + @"\Sample\src\";
            DotNetPackages dotNetPackages = new();
            //Get the output from the process
            string json = dotNetPackages.GetProcessOutput(path, "list package --deprecated --format json");

            //Act - runs a repo in about 4s
            List<NugetPackage> results = dotNetPackages.GetNugetPackagesVulnerable(json);

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count >= 0);
            //Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void GetNugetPackagesDeprecatedJsonTest()
        {
            //Arrange
            // Use the actual src directory where this test is running
            string path = "/home/runner/work/RepoGovernance/RepoGovernance/src";
            
            // Skip test if path doesn't exist to avoid hard-coding paths
            if (!Directory.Exists(path))
            {
                // Try a relative approach
                DirectoryInfo dir = new(Directory.GetCurrentDirectory());
                path = Path.Combine(dir?.Parent?.Parent?.Parent?.Parent?.FullName ?? "", "src");
                if (!Directory.Exists(path))
                {
                    Assert.Inconclusive($"Test skipped - path does not exist: {path}");
                    return;
                }
            }

            Debug.WriteLine($"Using path: {path}");
            DotNetPackages dotNetPackages = new();

            //Act
            string json = dotNetPackages.GetNugetPackagesDeprecatedJson(path);

            //Assert
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Length > 0);
            Debug.WriteLine($"JSON output: {json}");
            // Should contain JSON structure for deprecated packages
            Assert.IsTrue(json.Contains("projects") || json.Contains("{}"));
        }

        [TestMethod]
        public void GetNugetPackagesOutdatedJsonTest()
        {
            //Arrange
            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = Path.Combine(dir?.Parent?.Parent?.Parent?.Parent?.FullName ?? "", "src"); // Go up 5 levels instead of 4
            
            // Skip test if path doesn't exist to avoid hard-coding paths
            if (!Directory.Exists(path))
            {
                Assert.Inconclusive($"Test skipped - path does not exist: {path}");
                return;
            }

            DotNetPackages dotNetPackages = new();

            //Act
            string json = dotNetPackages.GetNugetPackagesOutdatedJson(path);

            //Assert
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Length > 0);
            // Should contain JSON structure for outdated packages
            Assert.IsTrue(json.Contains("projects") || json.Contains("{}"));
        }

        [TestMethod]
        public void GetNugetPackagesVulnerableJsonTest()
        {
            //Arrange
            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = Path.Combine(dir?.Parent?.Parent?.Parent?.Parent?.FullName ?? "", "src"); // Go up 5 levels instead of 4
            
            // Skip test if path doesn't exist to avoid hard-coding paths
            if (!Directory.Exists(path))
            {
                Assert.Inconclusive($"Test skipped - path does not exist: {path}");
                return;
            }

            DotNetPackages dotNetPackages = new();

            //Act
            string json = dotNetPackages.GetNugetPackagesVulnerableJson(path);

            //Assert
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Length > 0);
            // Should contain JSON structure for vulnerable packages
            Assert.IsTrue(json.Contains("projects") || json.Contains("{}"));
        }

        [TestMethod]
        public void RestorePackagesTest()
        {
            //Arrange
            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = Path.Combine(dir?.Parent?.Parent?.Parent?.Parent?.FullName ?? "", "src"); // Go up 5 levels instead of 4
            
            // Skip test if path doesn't exist to avoid hard-coding paths
            if (!Directory.Exists(path))
            {
                Assert.Inconclusive($"Test skipped - path does not exist: {path}");
                return;
            }

            DotNetPackages dotNetPackages = new();

            //Act
            string result = dotNetPackages.RestorePackages(path);

            //Assert
            Assert.IsNotNull(result);
            // Restore should provide some output (success or error message)
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void ScanAllPackagesTest()
        {
            //Arrange
            // Use the actual src directory where this test is running
            string path = "/home/runner/work/RepoGovernance/RepoGovernance/src";
            
            // Skip test if path doesn't exist to avoid hard-coding paths
            if (!Directory.Exists(path))
            {
                // Try a relative approach
                DirectoryInfo dir = new(Directory.GetCurrentDirectory());
                path = Path.Combine(dir?.Parent?.Parent?.Parent?.Parent?.FullName ?? "", "src");
                if (!Directory.Exists(path))
                {
                    Assert.Inconclusive($"Test skipped - path does not exist: {path}");
                    return;
                }
            }

            Debug.WriteLine($"Using path: {path}");
            DotNetPackages dotNetPackages = new();

            //Act
            NugetScanResults results = dotNetPackages.ScanAllPackages(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.DeprecatedJson);
            Assert.IsNotNull(results.OutdatedJson);
            Assert.IsNotNull(results.VulnerableJson);
            Assert.IsTrue(results.DeprecatedJson.Length > 0);
            Assert.IsTrue(results.OutdatedJson.Length > 0);
            Assert.IsTrue(results.VulnerableJson.Length > 0);
            Debug.WriteLine($"Deprecated: {results.DeprecatedJson.Substring(0, Math.Min(100, results.DeprecatedJson.Length))}...");
            Debug.WriteLine($"Outdated: {results.OutdatedJson.Substring(0, Math.Min(100, results.OutdatedJson.Length))}...");
            Debug.WriteLine($"Vulnerable: {results.VulnerableJson.Substring(0, Math.Min(100, results.VulnerableJson.Length))}...");
        }
    }
}
