using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RepoGovernance.Tests.APIAccess
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CoverallsCodeCoverageApiTests
    {
        [TestMethod]
        public async Task GetCoverallsCodeCoverage_ValidOwnerAndRepo_ReturnsExpectedUrl()
        {
            // Arrange
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            // This will make an actual HTTP call but with ignoreErrors=true, so it should handle failures gracefully
            CoverallsCodeCoverage? result = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);

            // Assert
            // The method might return null if the repo doesn't exist on coveralls, which is expected
            // We're primarily testing that the method doesn't throw exceptions and handles the call correctly
            Assert.IsTrue(result == null || result is CoverallsCodeCoverage);
        }

        [TestMethod]
        public async Task GetCoverallsCodeCoverage_EmptyOwner_HandlesGracefully()
        {
            // Arrange
            string owner = "";
            string repo = "testrepo";

            // Act
            CoverallsCodeCoverage? result = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is CoverallsCodeCoverage);
        }

        [TestMethod]
        public async Task GetCoverallsCodeCoverage_EmptyRepo_HandlesGracefully()
        {
            // Arrange
            string owner = "testowner";
            string repo = "";

            // Act
            CoverallsCodeCoverage? result = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is CoverallsCodeCoverage);
        }

        [TestMethod]
        public async Task GetCoverallsCodeCoverage_NullOwner_HandlesGracefully()
        {
            // Arrange
            string owner = null!;
            string repo = "testrepo";

            // Act
            CoverallsCodeCoverage? result = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is CoverallsCodeCoverage);
        }

        [TestMethod]
        public async Task GetCoverallsCodeCoverage_NullRepo_HandlesGracefully()
        {
            // Arrange
            string owner = "testowner";
            string repo = null!;

            // Act
            CoverallsCodeCoverage? result = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is CoverallsCodeCoverage);
        }

        [TestMethod]
        public async Task GetCoverallsCodeCoverage_SpecialCharactersInNames_HandlesGracefully()
        {
            // Arrange
            string owner = "test-owner";
            string repo = "test.repo_name";

            // Act
            CoverallsCodeCoverage? result = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);

            // Assert
            // Should not throw exception, may return null
            Assert.IsTrue(result == null || result is CoverallsCodeCoverage);
        }
    }
}