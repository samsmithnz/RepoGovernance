using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class IgnoredRecommendationsIntegrationTests : BaseAPIAccessTests
    {
        [TestMethod]
        public async Task IgnoreRecommendation_Integration_SavesAndRetrievesSuccessfully()
        {
            // Arrange
            string user = "integrationtestuser";
            string owner = "integrationtestowner";
            string repo = "integrationtestrepo";
            string recommendationType = "TestType";
            string recommendationDetails = "TestDetails";

            IgnoredRecommendationsDA ignoredRecommendationsDA = new IgnoredRecommendationsDA(AzureStorageConnectionString);

            // Act
            bool ignoreResult = await ignoredRecommendationsDA.IgnoreRecommendation(
                user, owner, repo, recommendationType, recommendationDetails);

            // Assert
            Assert.IsTrue(ignoreResult, "IgnoreRecommendation should return true when saving is successful.");

            // Act: Retrieve ignored recommendations for this user/repo
            List<IgnoredRecommendation> ignoredList = await ignoredRecommendationsDA.GetIgnoredRecommendations(user, owner, repo);

            // Assert: Validate that the recommendation is present
            Assert.IsNotNull(ignoredList, "Ignored recommendations list should not be null.");
            Assert.IsTrue(ignoredList.Exists(ir =>
                ir.Owner == owner &&
                ir.Repository == repo &&
                ir.RecommendationType == recommendationType &&
                ir.RecommendationDetails == recommendationDetails),
                "Ignored recommendation should be found in the list.");
        }
    }
}