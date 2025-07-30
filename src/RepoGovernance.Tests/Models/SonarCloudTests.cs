using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SonarCloudTests
    {
        [TestMethod]
        public void SonarCloud_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            var sonarCloud = new SonarCloud();

            // Assert
            Assert.IsNull(sonarCloud.CodeSmellsBadgeImage);
            Assert.IsNull(sonarCloud.CodeSmellsLink);
            Assert.IsNull(sonarCloud.BugsBadgeImage);
            Assert.IsNull(sonarCloud.BugsLink);
            Assert.IsNull(sonarCloud.LinesOfCodeBadgeImage);
            Assert.IsNull(sonarCloud.LinesOfCodeLink);
        }

        [TestMethod]
        public void SonarCloud_PropertySetters_WorkCorrectly()
        {
            // Arrange
            var sonarCloud = new SonarCloud();

            // Act
            sonarCloud.CodeSmellsBadgeImage = "https://example.com/code-smells.svg";
            sonarCloud.CodeSmellsLink = "https://example.com/code-smells";
            sonarCloud.BugsBadgeImage = "https://example.com/bugs.svg";
            sonarCloud.BugsLink = "https://example.com/bugs";
            sonarCloud.LinesOfCodeBadgeImage = "https://example.com/lines.svg";
            sonarCloud.LinesOfCodeLink = "https://example.com/lines";

            // Assert
            Assert.AreEqual("https://example.com/code-smells.svg", sonarCloud.CodeSmellsBadgeImage);
            Assert.AreEqual("https://example.com/code-smells", sonarCloud.CodeSmellsLink);
            Assert.AreEqual("https://example.com/bugs.svg", sonarCloud.BugsBadgeImage);
            Assert.AreEqual("https://example.com/bugs", sonarCloud.BugsLink);
            Assert.AreEqual("https://example.com/lines.svg", sonarCloud.LinesOfCodeBadgeImage);
            Assert.AreEqual("https://example.com/lines", sonarCloud.LinesOfCodeLink);
        }

        [TestMethod]
        public void SonarCloud_PropertySetters_HandleNullValues()
        {
            // Arrange
            var sonarCloud = new SonarCloud();
            sonarCloud.CodeSmellsBadgeImage = "test";
            sonarCloud.CodeSmellsLink = "test";
            sonarCloud.BugsBadgeImage = "test";
            sonarCloud.BugsLink = "test";
            sonarCloud.LinesOfCodeBadgeImage = "test";
            sonarCloud.LinesOfCodeLink = "test";

            // Act
            sonarCloud.CodeSmellsBadgeImage = null;
            sonarCloud.CodeSmellsLink = null;
            sonarCloud.BugsBadgeImage = null;
            sonarCloud.BugsLink = null;
            sonarCloud.LinesOfCodeBadgeImage = null;
            sonarCloud.LinesOfCodeLink = null;

            // Assert
            Assert.IsNull(sonarCloud.CodeSmellsBadgeImage);
            Assert.IsNull(sonarCloud.CodeSmellsLink);
            Assert.IsNull(sonarCloud.BugsBadgeImage);
            Assert.IsNull(sonarCloud.BugsLink);
            Assert.IsNull(sonarCloud.LinesOfCodeBadgeImage);
            Assert.IsNull(sonarCloud.LinesOfCodeLink);
        }

        [TestMethod]
        public void SonarCloud_PropertySetters_HandleEmptyStrings()
        {
            // Arrange
            var sonarCloud = new SonarCloud();

            // Act
            sonarCloud.CodeSmellsBadgeImage = "";
            sonarCloud.CodeSmellsLink = "";
            sonarCloud.BugsBadgeImage = "";
            sonarCloud.BugsLink = "";
            sonarCloud.LinesOfCodeBadgeImage = "";
            sonarCloud.LinesOfCodeLink = "";

            // Assert
            Assert.AreEqual("", sonarCloud.CodeSmellsBadgeImage);
            Assert.AreEqual("", sonarCloud.CodeSmellsLink);
            Assert.AreEqual("", sonarCloud.BugsBadgeImage);
            Assert.AreEqual("", sonarCloud.BugsLink);
            Assert.AreEqual("", sonarCloud.LinesOfCodeBadgeImage);
            Assert.AreEqual("", sonarCloud.LinesOfCodeLink);
        }
    }
}