using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models.NuGetPackages;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
            string path = @"C:\Users\samsm\source\repos\RepoGovernance\src\RepoGovernance.Tests\Sample\RepoAutomationUnitTests\";
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesDeprecated(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }


        [TestMethod]
        public void NugetPackagesOutdatedTest()
        {
            //Arrange
            string path = @"C:\Users\samsm\source\repos\RepoGovernance\src\RepoGovernance.Tests\Sample\RepoAutomationUnitTests\";
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesOutdated(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
        }


        [TestMethod]
        public void NugetPackagesVulnerableTest()
        {
            //Arrange
            string path = @"C:\Users\samsm\source\repos\RepoGovernance\src\RepoGovernance.Tests\Sample\RepoAutomationUnitTests\";
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesVulnerable(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }
    }
}
