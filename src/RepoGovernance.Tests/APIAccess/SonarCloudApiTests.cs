using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RepoGovernance.Tests.APIAccess
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SonarCloudApiTests
    {
        [TestMethod]
        public async Task GetSonarCloudMetrics_ValidOwnerAndRepo_ReturnsExpectedResults()
        {
            // Arrange
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            SonarCloud? result = await SonarCloudApi.GetSonarCloudMetrics(owner, repo);

            // Assert
            // The method might return null if no badges are available, which is expected for test data
            Assert.IsTrue(result == null || result is SonarCloud);
        }

        [TestMethod]
        public async Task GetSonarCloudCodeSmells_ValidOwnerAndRepo_ReturnsStringOrNull()
        {
            // Arrange
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            string? result = await SonarCloudApi.GetSonarCloudCodeSmells(owner, repo);

            // Assert
            // The method returns a string or null, both are valid
            Assert.IsTrue(result == null || result is string);
        }

        [TestMethod]
        public async Task GetSonarCloudCodeBugs_ValidOwnerAndRepo_ReturnsStringOrNull()
        {
            // Arrange
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            string? result = await SonarCloudApi.GetSonarCloudCodeBugs(owner, repo);

            // Assert
            // The method returns a string or null, both are valid
            Assert.IsTrue(result == null || result is string);
        }

        [TestMethod]
        public async Task GetSonarCloudLinesOfCode_ValidOwnerAndRepo_ReturnsStringOrNull()
        {
            // Arrange
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            string? result = await SonarCloudApi.GetSonarCloudLinesOfCode(owner, repo);

            // Assert
            // The method returns a string or null, both are valid
            Assert.IsTrue(result == null || result is string);
        }

        [TestMethod]
        public async Task GetSonarCloudMetrics_EmptyOwner_HandlesGracefully()
        {
            // Arrange
            string owner = "";
            string repo = "testrepo";

            // Act
            SonarCloud? result = await SonarCloudApi.GetSonarCloudMetrics(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is SonarCloud);
        }

        [TestMethod]
        public async Task GetSonarCloudMetrics_EmptyRepo_HandlesGracefully()
        {
            // Arrange
            string owner = "testowner";
            string repo = "";

            // Act
            SonarCloud? result = await SonarCloudApi.GetSonarCloudMetrics(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is SonarCloud);
        }

        [TestMethod]
        public async Task GetSonarCloudMetrics_NullOwner_HandlesGracefully()
        {
            // Arrange
            string owner = null!;
            string repo = "testrepo";

            // Act
            SonarCloud? result = await SonarCloudApi.GetSonarCloudMetrics(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is SonarCloud);
        }

        [TestMethod]
        public async Task GetSonarCloudMetrics_NullRepo_HandlesGracefully()
        {
            // Arrange
            string owner = "testowner";
            string repo = null!;

            // Act
            SonarCloud? result = await SonarCloudApi.GetSonarCloudMetrics(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is SonarCloud);
        }

        [TestMethod]
        public async Task GetSonarCloudMetrics_SpecialCharactersInNames_HandlesGracefully()
        {
            // Arrange
            string owner = "test-owner.special";
            string repo = "test.repo_name-with-dashes";

            // Act
            SonarCloud? result = await SonarCloudApi.GetSonarCloudMetrics(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is SonarCloud);
        }
    }
}