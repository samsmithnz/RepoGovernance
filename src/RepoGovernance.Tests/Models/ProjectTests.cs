using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ProjectTests
    {
        [TestMethod]
        public void Project_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            Project project = new Project();

            // Assert
            Assert.IsNull(project.FileName);
            Assert.IsNull(project.Path);
            Assert.IsNull(project.Content);
            Assert.IsNull(project.Framework);
            Assert.IsNull(project.Color);
        }

        [TestMethod]
        public void Project_PropertySetters_WorkCorrectly()
        {
            // Arrange
            Project project = new Project();

            // Act
            project.FileName = "test.csproj";
            project.Path = "/path/to/project";
            project.Content = "<Project>...</Project>";
            project.Framework = "net8.0";
            project.Color = "bg-primary";

            // Assert
            Assert.AreEqual("test.csproj", project.FileName);
            Assert.AreEqual("/path/to/project", project.Path);
            Assert.AreEqual("<Project>...</Project>", project.Content);
            Assert.AreEqual("net8.0", project.Framework);
            Assert.AreEqual("bg-primary", project.Color);
        }

        [TestMethod]
        public void Project_PropertySetters_HandleNullValues()
        {
            // Arrange
            Project project = new Project();
            project.FileName = "test";
            project.Path = "test";
            project.Content = "test";
            project.Framework = "test";
            project.Color = "test";

            // Act
            project.FileName = null;
            project.Path = null;
            project.Content = null;
            project.Framework = null;
            project.Color = null;

            // Assert
            Assert.IsNull(project.FileName);
            Assert.IsNull(project.Path);
            Assert.IsNull(project.Content);
            Assert.IsNull(project.Framework);
            Assert.IsNull(project.Color);
        }

        [TestMethod]
        public void Project_PropertySetters_HandleEmptyStrings()
        {
            // Arrange
            Project project = new Project();

            // Act
            project.FileName = "";
            project.Path = "";
            project.Content = "";
            project.Framework = "";
            project.Color = "";

            // Assert
            Assert.AreEqual("", project.FileName);
            Assert.AreEqual("", project.Path);
            Assert.AreEqual("", project.Content);
            Assert.AreEqual("", project.Framework);
            Assert.AreEqual("", project.Color);
        }

        [TestMethod]
        public void Project_PropertySetters_HandleSpecialCharacters()
        {
            // Arrange
            Project project = new Project();

            // Act
            project.FileName = "test-file.csproj";
            project.Path = "/path/with spaces/project";
            project.Content = "<Project Sdk=\"Microsoft.NET.Sdk\">";
            project.Framework = ".NET 8.0";
            project.Color = "bg-success";

            // Assert
            Assert.AreEqual("test-file.csproj", project.FileName);
            Assert.AreEqual("/path/with spaces/project", project.Path);
            Assert.AreEqual("<Project Sdk=\"Microsoft.NET.Sdk\">", project.Content);
            Assert.AreEqual(".NET 8.0", project.Framework);
            Assert.AreEqual("bg-success", project.Color);
        }
    }
}