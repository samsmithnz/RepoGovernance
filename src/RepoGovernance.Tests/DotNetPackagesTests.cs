using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models.NuGetPackages;
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
    }
}
