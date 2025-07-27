using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using System.Threading.Tasks;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class GitHubSecurityApiTests
    {
        [TestMethod]
        public async Task GetSecurityAlertsCount_WithoutCredentials_ShouldReturnZero()
        {
            // Arrange & Act
            int result = await GitHubSecurityApi.GetSecurityAlertsCount(null, null, "test", "test");

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task GetSecurityAlertsCount_WithEmptyCredentials_ShouldReturnZero()
        {
            // Arrange & Act
            int result = await GitHubSecurityApi.GetSecurityAlertsCount("", "", "test", "test");

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}