using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Models;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class TaskItemTests
    {
        [TestMethod]
        public void Constructor_ValidParameters_CreatesInstanceWithId()
        {
            // Arrange
            string owner = "testowner";
            string repository = "testrepo";
            string recommendationType = "Repository Settings";
            string recommendationDetails = "Enable branch protection";
            string expectedId = "testowner|testrepo|Repository Settings|Enable branch protection";

            // Act
            TaskItem taskItem = new TaskItem(owner, repository, recommendationType, recommendationDetails);

            // Assert
            Assert.AreEqual(owner, taskItem.Owner);
            Assert.AreEqual(repository, taskItem.Repository);
            Assert.AreEqual(recommendationType, taskItem.RecommendationType);
            Assert.AreEqual(recommendationDetails, taskItem.RecommendationDetails);
            Assert.AreEqual(expectedId, taskItem.Id);
        }

        [TestMethod]
        public void Constructor_EmptyParameters_CreatesInstanceWithEmptyId()
        {
            // Arrange
            string owner = "";
            string repository = "";
            string recommendationType = "";
            string recommendationDetails = "";
            string expectedId = "|||";

            // Act
            TaskItem taskItem = new TaskItem(owner, repository, recommendationType, recommendationDetails);

            // Assert
            Assert.AreEqual(owner, taskItem.Owner);
            Assert.AreEqual(repository, taskItem.Repository);
            Assert.AreEqual(recommendationType, taskItem.RecommendationType);
            Assert.AreEqual(recommendationDetails, taskItem.RecommendationDetails);
            Assert.AreEqual(expectedId, taskItem.Id);
        }

        [TestMethod]
        public void Constructor_SpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            string owner = "test-owner";
            string repository = "test_repo.net";
            string recommendationType = "Branch Policies";
            string recommendationDetails = "Enable admin enforcement";
            string expectedId = "test-owner|test_repo.net|Branch Policies|Enable admin enforcement";

            // Act
            TaskItem taskItem = new TaskItem(owner, repository, recommendationType, recommendationDetails);

            // Assert
            Assert.AreEqual(expectedId, taskItem.Id);
        }

        [TestMethod]
        public void Id_ConsistentWithIgnoredRecommendation()
        {
            // Arrange
            string owner = "testowner";
            string repository = "testrepo";
            string recommendationType = "Security";
            string recommendationDetails = "Fix vulnerability";

            // Act
            TaskItem taskItem = new TaskItem(owner, repository, recommendationType, recommendationDetails);
            IgnoredRecommendation ignoredRec = new IgnoredRecommendation("user", owner, repository, recommendationType, recommendationDetails);

            // Assert - IDs should be identical (user part is not included in TaskItem ID)
            Assert.AreEqual(taskItem.Id, ignoredRec.GetUniqueId());
        }
    }
}