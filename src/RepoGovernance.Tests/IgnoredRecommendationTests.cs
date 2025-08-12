using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;
using System;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class IgnoredRecommendationTests
    {
        [TestMethod]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            string user = "testuser";
            string owner = "testowner";
            string repository = "testrepo";
            string recommendationType = "Repository Settings";
            string recommendationDetails = "Enable branch protection";

            // Act
            IgnoredRecommendation ignoredRecommendation = new IgnoredRecommendation(user, owner, repository, recommendationType, recommendationDetails);

            // Assert
            Assert.AreEqual(user, ignoredRecommendation.User);
            Assert.AreEqual(owner, ignoredRecommendation.Owner);
            Assert.AreEqual(repository, ignoredRecommendation.Repository);
            Assert.AreEqual(recommendationType, ignoredRecommendation.RecommendationType);
            Assert.AreEqual(recommendationDetails, ignoredRecommendation.RecommendationDetails);
            Assert.IsTrue(ignoredRecommendation.IgnoredDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [TestMethod]
        public void ParameterlessConstructor_CreatesInstanceWithDefaults()
        {
            // Act
            IgnoredRecommendation ignoredRecommendation = new IgnoredRecommendation();

            // Assert
            Assert.AreEqual("", ignoredRecommendation.User);
            Assert.AreEqual("", ignoredRecommendation.Owner);
            Assert.AreEqual("", ignoredRecommendation.Repository);
            Assert.AreEqual("", ignoredRecommendation.RecommendationType);
            Assert.AreEqual("", ignoredRecommendation.RecommendationDetails);
            Assert.IsTrue(ignoredRecommendation.IgnoredDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [TestMethod]
        public void GetUniqueId_ValidData_ReturnsExpectedFormat()
        {
            // Arrange
            IgnoredRecommendation ignoredRecommendation = new IgnoredRecommendation("user", "owner", "repo", "Security", "Fix vulnerability");
            string expected = "owner|repo|Security|Fix vulnerability";

            // Act
            string actual = ignoredRecommendation.GetUniqueId();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUniqueId_EmptyData_ReturnsCorrectFormat()
        {
            // Arrange
            IgnoredRecommendation ignoredRecommendation = new IgnoredRecommendation();
            string expected = "|||";

            // Act
            string actual = ignoredRecommendation.GetUniqueId();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUniqueId_SpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            IgnoredRecommendation ignoredRecommendation = new IgnoredRecommendation("user", "test-owner", "test_repo.net", "Branch Policies", "Enable admin enforcement");
            string expected = "test-owner|test_repo.net|Branch Policies|Enable admin enforcement";

            // Act
            string actual = ignoredRecommendation.GetUniqueId();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}