using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;
using RepoGovernance.Core.Models.NuGetPackages;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class NuGetProcessingTests
    {
        [TestMethod]
        public void ProcessNuGetPackagePayload_WithValidDeprecatedData_ShouldProcessCorrectly()
        {
            //Arrange
            SummaryItem summaryItem = new("testuser", "testowner", "testrepo");
            string deprecatedPayload = @"{
                ""projects"": [
                    {
                        ""path"": ""test.csproj"",
                        ""frameworks"": [
                            {
                                ""framework"": ""net8.0"",
                                ""topLevelPackages"": [
                                    {
                                        ""id"": ""TestPackage"",
                                        ""requestedVersion"": ""1.0.0"",
                                        ""resolvedVersion"": ""1.0.0"",
                                        ""latestVersion"": ""2.0.0""
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }";

            //Act - Use reflection to call the private method
            MethodInfo? method = typeof(SummaryItemsDA).GetMethod("ProcessNuGetPackagePayload", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method, "ProcessNuGetPackagePayload method should exist");
            
            List<NugetPackage>? result = (List<NugetPackage>?)method.Invoke(null, new object[] { summaryItem, deprecatedPayload, "Deprecated" });

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TestPackage", result[0].PackageId);
            Assert.AreEqual("Deprecated", result[0].Type);
            Assert.AreEqual("net8.0", result[0].Framework);
            
            // Verify the package was added to the summary item
            Assert.AreEqual(1, summaryItem.NuGetPackages.Count);
            Assert.AreEqual("Deprecated", summaryItem.NuGetPackages[0].Type);
        }

        [TestMethod]
        public void ProcessNuGetPackagePayload_WithNullPayload_ShouldReturnNull()
        {
            //Arrange
            SummaryItem summaryItem = new("testuser", "testowner", "testrepo");

            //Act - Use reflection to call the private method
            MethodInfo? method = typeof(SummaryItemsDA).GetMethod("ProcessNuGetPackagePayload", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method, "ProcessNuGetPackagePayload method should exist");
            
            List<NugetPackage>? result = (List<NugetPackage>?)method.Invoke(null, new object?[] { summaryItem, null, "Deprecated" });

            //Assert
            Assert.IsNull(result);
            Assert.AreEqual(0, summaryItem.NuGetPackages.Count);
        }

        [TestMethod]
        public void ProcessNuGetPackagePayload_WithMultipleTypes_ShouldReplaceExistingType()
        {
            //Arrange
            SummaryItem summaryItem = new("testuser", "testowner", "testrepo");
            
            // Add an existing deprecated package
            summaryItem.NuGetPackages.Add(new NugetPackage
            {
                PackageId = "OldPackage",
                Type = "Deprecated",
                Framework = "net8.0",
                Path = "old.csproj",
                PackageVersion = "1.0.0"
            });

            string deprecatedPayload = @"{
                ""projects"": [
                    {
                        ""path"": ""test.csproj"",
                        ""frameworks"": [
                            {
                                ""framework"": ""net8.0"",
                                ""topLevelPackages"": [
                                    {
                                        ""id"": ""NewPackage"",
                                        ""requestedVersion"": ""2.0.0"",
                                        ""resolvedVersion"": ""2.0.0"",
                                        ""latestVersion"": ""3.0.0""
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }";

            //Act - Use reflection to call the private method
            MethodInfo? method = typeof(SummaryItemsDA).GetMethod("ProcessNuGetPackagePayload", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method, "ProcessNuGetPackagePayload method should exist");
            
            List<NugetPackage>? result = (List<NugetPackage>?)method.Invoke(null, new object[] { summaryItem, deprecatedPayload, "Deprecated" });

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            
            // Verify old deprecated package was removed and new one added
            Assert.AreEqual(1, summaryItem.NuGetPackages.Count);
            Assert.AreEqual("NewPackage", summaryItem.NuGetPackages[0].PackageId);
            Assert.AreEqual("Deprecated", summaryItem.NuGetPackages[0].Type);
        }
    }
}