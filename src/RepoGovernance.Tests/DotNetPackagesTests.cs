using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using RepoGovernance.Core;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using RepoGovernance.Core.Helpers;
using System.Collections.Generic;
using RepoGovernance.Core.Models.NuGetPackages;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DotNetPackagesTests
    {
        [TestMethod]
        public void NugetPackagesOutdatedTest()
        {
            //Arrange
            string path = "C:\\Users\\samsm\\source\\repos\\DotNetCensus\\src";
            DotNetPackages dotNetPackages = new();

            //Act - runs a repo in about 4s
            List<NugetResult> results = dotNetPackages.GetNugetPackagesOutdated(path);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
        }
    }
}
