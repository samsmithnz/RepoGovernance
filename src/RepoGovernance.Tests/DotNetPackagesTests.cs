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
            string path = dir.Parent.Parent.Parent.FullName + @"\Sample\src\";
            Debug.WriteLine("Path to test");
            Debug.WriteLine(path);
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesDeprecated(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }


        [TestMethod]
        public void NugetPackagesOutdatedTest()
        {
            //Arrange
            System.IO.DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = dir.Parent.Parent.Parent.FullName + @"\Sample\src\";
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesOutdated(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(6, results.Count);
        }


        [TestMethod]
        public void NugetPackagesVulnerableTest()
        {
            //Arrange
            System.IO.DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            string path = dir.Parent.Parent.Parent.FullName + @"\Sample\src\";
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesVulnerable(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }
    }
}
