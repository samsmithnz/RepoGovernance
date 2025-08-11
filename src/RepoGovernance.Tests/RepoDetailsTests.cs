using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Web.Models;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class RepoDetailsTests
    {
        [TestMethod]
        public void ParameterlessConstructor_InitializesWithDefaults()
        {
            // Act
            RepoDetails repoDetails = new RepoDetails();

            // Assert
            Assert.AreEqual("", repoDetails.Owner);
            Assert.AreEqual("", repoDetails.Repository);
            Assert.IsNotNull(repoDetails.Recommendations);
            Assert.IsNotNull(repoDetails.IgnoredRecommendations);
            Assert.AreEqual(0, repoDetails.Recommendations.Count);
            Assert.AreEqual(0, repoDetails.IgnoredRecommendations.Count);
            Assert.IsFalse(repoDetails.IsContributor);
        }

        [TestMethod]
        public void Constructor_WithParameters_InitializesCorrectly()
        {
            // Arrange
            string owner = "testowner";
            string repository = "testrepo";

            // Act
            RepoDetails repoDetails = new RepoDetails(owner, repository);

            // Assert
            Assert.AreEqual(owner, repoDetails.Owner);
            Assert.AreEqual(repository, repoDetails.Repository);
            Assert.IsNotNull(repoDetails.Recommendations);
            Assert.IsNotNull(repoDetails.IgnoredRecommendations);
            Assert.AreEqual(0, repoDetails.Recommendations.Count);
            Assert.AreEqual(0, repoDetails.IgnoredRecommendations.Count);
            Assert.IsFalse(repoDetails.IsContributor);
        }

        [TestMethod]
        public void TotalRecommendations_CalculatesCorrectly()
        {
            // Arrange
            RepoDetails repoDetails = new RepoDetails("owner", "repo");
            repoDetails.Recommendations.Add(new TaskItem("owner", "repo", "Security", "Fix vulnerability"));
            repoDetails.Recommendations.Add(new TaskItem("owner", "repo", "Branch Policies", "Add protection"));
            repoDetails.IgnoredRecommendations.Add(new TaskItem("owner", "repo", "Actions", "Add workflow"));

            // Act & Assert
            Assert.AreEqual(3, repoDetails.TotalRecommendations);
        }

        [TestMethod]
        public void ActiveRecommendations_ReturnsCorrectCount()
        {
            // Arrange
            RepoDetails repoDetails = new RepoDetails("owner", "repo");
            repoDetails.Recommendations.Add(new TaskItem("owner", "repo", "Security", "Fix vulnerability"));
            repoDetails.Recommendations.Add(new TaskItem("owner", "repo", "Branch Policies", "Add protection"));
            repoDetails.IgnoredRecommendations.Add(new TaskItem("owner", "repo", "Actions", "Add workflow"));

            // Act & Assert
            Assert.AreEqual(2, repoDetails.ActiveRecommendations);
        }

        [TestMethod]
        public void IgnoredCount_ReturnsCorrectCount()
        {
            // Arrange
            RepoDetails repoDetails = new RepoDetails("owner", "repo");
            repoDetails.Recommendations.Add(new TaskItem("owner", "repo", "Security", "Fix vulnerability"));
            repoDetails.IgnoredRecommendations.Add(new TaskItem("owner", "repo", "Actions", "Add workflow"));
            repoDetails.IgnoredRecommendations.Add(new TaskItem("owner", "repo", "Dependabot", "Update config"));

            // Act & Assert
            Assert.AreEqual(2, repoDetails.IgnoredCount);
        }

        [TestMethod]
        public void EmptyLists_PropertiesReturnZero()
        {
            // Arrange
            RepoDetails repoDetails = new RepoDetails("owner", "repo");

            // Act & Assert
            Assert.AreEqual(0, repoDetails.TotalRecommendations);
            Assert.AreEqual(0, repoDetails.ActiveRecommendations);
            Assert.AreEqual(0, repoDetails.IgnoredCount);
        }
    }
}