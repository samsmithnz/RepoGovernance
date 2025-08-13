using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GitHubRepositoryRulesApiTests
    {
        [TestMethod]
        public async Task HasRepositoryRulesProtection_WithNullClientCredentials_ReturnsFalse()
        {
            // Arrange
            string? clientId = null;
            string? clientSecret = null;
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            bool result = await GitHubRepositoryRulesApi.HasRepositoryRulesProtection(clientId, clientSecret, owner, repo);

            // Assert
            Assert.IsFalse(result, "Should return false when client credentials are null");
        }

        [TestMethod]
        public async Task HasRepositoryRulesProtection_WithEmptyClientCredentials_ReturnsFalse()
        {
            // Arrange
            string clientId = "";
            string clientSecret = "";
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            bool result = await GitHubRepositoryRulesApi.HasRepositoryRulesProtection(clientId, clientSecret, owner, repo);

            // Assert
            Assert.IsFalse(result, "Should return false when client credentials are empty");
        }

        [TestMethod]
        public async Task HasRepositoryRulesProtection_WithValidCredentialsButNoRules_ReturnsFalse()
        {
            // Arrange - Using dummy credentials that will likely fail authentication
            // This tests the graceful error handling
            string clientId = "dummy_client_id";
            string clientSecret = "dummy_client_secret";
            string owner = "nonexistent";
            string repo = "nonexistent";

            // Act
            bool result = await GitHubRepositoryRulesApi.HasRepositoryRulesProtection(clientId, clientSecret, owner, repo);

            // Assert
            Assert.IsFalse(result, "Should return false when no repository rules are found or API call fails");
        }

        [TestMethod]
        public async Task GetRepositoryRulesets_WithNullCredentials_ReturnsEmptyList()
        {
            // Arrange
            string? clientId = null;
            string? clientSecret = null;
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            List<RepositoryRuleset>? result = await GitHubRepositoryRulesApi.GetRepositoryRulesets(clientId, clientSecret, owner, repo);

            // Assert
            Assert.IsNotNull(result, "Should return empty list, not null");
            Assert.AreEqual(0, result.Count, "Should return empty list when credentials are null");
        }

        [TestMethod]
        public async Task GetRepositoryRulesets_WithEmptyCredentials_ReturnsEmptyList()
        {
            // Arrange
            string clientId = "";
            string clientSecret = "";
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            List<RepositoryRuleset>? result = await GitHubRepositoryRulesApi.GetRepositoryRulesets(clientId, clientSecret, owner, repo);

            // Assert
            Assert.IsNotNull(result, "Should return empty list, not null");
            Assert.AreEqual(0, result.Count, "Should return empty list when credentials are empty");
        }
    }
}