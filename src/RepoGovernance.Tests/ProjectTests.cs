using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Tests.Models
{
    [TestClass]
    public class ProjectTests
    {
        [TestMethod]
        public void Project_PropertySettersAndGetters_WorkCorrectly()
        {
            // Arrange
            var project = new Project();

            // Act
            project.FileName = "MyProject.csproj";
            project.Path = "/src/MyProject/MyProject.csproj";
            project.Content = "<Project Sdk=\"Microsoft.NET.Sdk\"></Project>";
            project.Framework = ".NET 8.0";
            project.Color = "bg-primary";

            // Assert
            Assert.AreEqual("MyProject.csproj", project.FileName);
            Assert.AreEqual("/src/MyProject/MyProject.csproj", project.Path);
            Assert.AreEqual("<Project Sdk=\"Microsoft.NET.Sdk\"></Project>", project.Content);
            Assert.AreEqual(".NET 8.0", project.Framework);
            Assert.AreEqual("bg-primary", project.Color);
        }
    }
}
