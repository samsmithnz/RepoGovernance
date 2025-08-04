using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SummaryItemTests
    {
        [TestMethod]
        public void SummaryItem_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string user = "testuser";
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            SummaryItem summaryItem = new SummaryItem(user, owner, repo);

            // Assert
            Assert.AreEqual(user, summaryItem.User);
            Assert.AreEqual(owner, summaryItem.Owner);
            Assert.AreEqual(repo, summaryItem.Repo);
            Assert.IsNotNull(summaryItem.RepoSettings);
            Assert.IsNotNull(summaryItem.RepoSettingsRecommendations);
            Assert.IsNotNull(summaryItem.Actions);
            Assert.IsNotNull(summaryItem.ActionRecommendations);
            Assert.IsNotNull(summaryItem.Dependabot);
            Assert.IsNotNull(summaryItem.DependabotRecommendations);
            Assert.IsNotNull(summaryItem.BranchPoliciesRecommendations);
            Assert.IsNotNull(summaryItem.GitVersion);
            Assert.IsNotNull(summaryItem.GitVersionRecommendations);
            Assert.IsNotNull(summaryItem.DotNetFrameworks);
            Assert.IsNotNull(summaryItem.DotNetFrameworksRecommendations);
            Assert.IsNotNull(summaryItem.PullRequests);
            Assert.IsNotNull(summaryItem.RepoLanguages);
            Assert.IsNotNull(summaryItem.NuGetPackages);
            Assert.AreEqual(0, summaryItem.SecurityIssuesCount);
            Assert.IsNull(summaryItem.AzureDeployment);
        }

        [TestMethod]
        public void SummaryItem_Constructor_SetsLastUpdatedToCurrentTime()
        {
            // Arrange
            DateTime beforeCreation = DateTime.Now;
            
            // Act
            SummaryItem summaryItem = new SummaryItem("user", "owner", "repo");
            DateTime afterCreation = DateTime.Now;

            // Assert
            Assert.IsTrue(summaryItem.LastUpdated >= beforeCreation);
            Assert.IsTrue(summaryItem.LastUpdated <= afterCreation);
        }

        [TestMethod]
        public void SummaryItem_Constructor_EmptyStrings_HandledCorrectly()
        {
            // Arrange
            string user = "";
            string owner = "";
            string repo = "";

            // Act
            SummaryItem summaryItem = new SummaryItem(user, owner, repo);

            // Assert
            Assert.AreEqual(user, summaryItem.User);
            Assert.AreEqual(owner, summaryItem.Owner);
            Assert.AreEqual(repo, summaryItem.Repo);
        }

        [TestMethod]
        public void SummaryItem_Constructor_NullStrings_HandledCorrectly()
        {
            // Arrange
            string user = null!;
            string owner = null!;
            string repo = null!;

            // Act
            SummaryItem summaryItem = new SummaryItem(user, owner, repo);

            // Assert
            Assert.AreEqual(user, summaryItem.User);
            Assert.AreEqual(owner, summaryItem.Owner);
            Assert.AreEqual(repo, summaryItem.Repo);
        }

        [TestMethod]
        public void SummaryItem_Properties_CanBeSetAndGet()
        {
            // Arrange
            SummaryItem summaryItem = new SummaryItem("user", "owner", "repo");

            // Act
            summaryItem.SecurityIssuesCount = 5;
            summaryItem.Actions.Add("test-action");
            summaryItem.ActionRecommendations.Add("test-recommendation");

            // Assert
            Assert.AreEqual(5, summaryItem.SecurityIssuesCount);
            Assert.IsTrue(summaryItem.Actions.Contains("test-action"));
            Assert.IsTrue(summaryItem.ActionRecommendations.Contains("test-recommendation"));
        }

        [TestMethod]
        public void SummaryItem_ListProperties_AreInitializedEmpty()
        {
            // Arrange & Act
            SummaryItem summaryItem = new SummaryItem("user", "owner", "repo");

            // Assert
            Assert.AreEqual(0, summaryItem.RepoSettingsRecommendations.Count);
            Assert.AreEqual(0, summaryItem.Actions.Count);
            Assert.AreEqual(0, summaryItem.ActionRecommendations.Count);
            Assert.AreEqual(0, summaryItem.Dependabot.Count);
            Assert.AreEqual(0, summaryItem.DependabotRecommendations.Count);
            Assert.AreEqual(0, summaryItem.BranchPoliciesRecommendations.Count);
            Assert.AreEqual(0, summaryItem.GitVersion.Count);
            Assert.AreEqual(0, summaryItem.GitVersionRecommendations.Count);
            Assert.AreEqual(0, summaryItem.DotNetFrameworks.Count);
            Assert.AreEqual(0, summaryItem.DotNetFrameworksRecommendations.Count);
            Assert.AreEqual(0, summaryItem.PullRequests.Count);
            Assert.AreEqual(0, summaryItem.RepoLanguages.Count);
            Assert.AreEqual(0, summaryItem.NuGetPackages.Count);
        }
    }
}