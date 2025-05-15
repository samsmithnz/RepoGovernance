using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System;

namespace RepoGovernance.Tests.Models
{
    [TestClass]
    public class DORASummaryItemTests
    {
        [TestMethod]
        public void DORASummaryItem_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            var item = new DORASummaryItem();

            // Assert
            Assert.IsNull(item.Owner);
            Assert.IsNull(item.Project);
            Assert.IsNull(item.Repo);
            Assert.AreEqual(0, item.NumberOfDays);
            Assert.AreEqual(0, item.DeploymentFrequency);
            Assert.IsNull(item.DeploymentFrequencyBadgeURL);
            Assert.IsNull(item.DeploymentFrequencyBadgeWithMetricURL);
            Assert.AreEqual(0, item.LeadTimeForChanges);
            Assert.IsNull(item.LeadTimeForChangesBadgeURL);
            Assert.IsNull(item.LeadTimeForChangesBadgeWithMetricURL);
            Assert.AreEqual(0, item.MeanTimeToRestore);
            Assert.IsNull(item.MeanTimeToRestoreBadgeURL);
            Assert.IsNull(item.MeanTimeToRestoreBadgeWithMetricURL);
            Assert.AreEqual(0, item.ChangeFailureRate);
            Assert.IsNull(item.ChangeFailureRateBadgeURL);
            Assert.IsNull(item.ChangeFailureRateBadgeWithMetricURL);
            Assert.IsNull(item.LastUpdatedMessage);
            Assert.AreEqual(default(DateTime), item.LastUpdated);
        }

        [TestMethod]
        public void DORASummaryItem_ParameterizedConstructor_SetsDefaultsAndProperties()
        {
            // Arrange
            string owner = "samsmithnz";
            string project = "MyProject";
            string repo = "MyRepo";

            // Act
            var item = new DORASummaryItem(owner, project, repo);

            // Assert
            Assert.AreEqual(owner, item.Owner);
            Assert.AreEqual(project, item.Project);
            Assert.AreEqual(repo, item.Repo);

            Assert.AreEqual(0, item.DeploymentFrequency);
            Assert.AreEqual("https://img.shields.io/badge/Deployment%20frequency-None-lightgrey", item.DeploymentFrequencyBadgeURL);
            Assert.AreEqual("https://img.shields.io/badge/Deployment%20frequency%20-None-lightgrey", item.DeploymentFrequencyBadgeWithMetricURL);
            Assert.AreEqual(0, item.LeadTimeForChanges);
            Assert.AreEqual("https://img.shields.io/badge/Lead%20time%20for%20changes-None-lightgrey", item.LeadTimeForChangesBadgeURL);
            Assert.AreEqual("https://img.shields.io/badge/Lead%20time%20for%20changes%20-None-lightgrey", item.LeadTimeForChangesBadgeWithMetricURL);
            Assert.AreEqual(0, item.MeanTimeToRestore);
            Assert.AreEqual("https://img.shields.io/badge/Time%20to%20restore%20service-None-lightgrey", item.MeanTimeToRestoreBadgeURL);
            Assert.AreEqual("https://img.shields.io/badge/Time%20to%20restore%20service%20-None-lightgrey", item.MeanTimeToRestoreBadgeWithMetricURL);
            Assert.AreEqual(0, item.ChangeFailureRate);
            Assert.AreEqual("https://img.shields.io/badge/Change%20failure%20rate-None-lightgrey", item.ChangeFailureRateBadgeURL);
            Assert.AreEqual("https://img.shields.io/badge/Change%20failure%20rate%20-None-lightgrey", item.ChangeFailureRateBadgeWithMetricURL);
            Assert.AreEqual("No data available", item.LastUpdatedMessage);
        }

        [TestMethod]
        public void DORASummaryItem_PropertySetters_WorkCorrectly()
        {
            // Arrange
            var item = new DORASummaryItem();

            // Act
            item.Owner = "owner";
            item.Project = "project";
            item.Repo = "repo";
            item.NumberOfDays = 30;
            item.DeploymentFrequency = 5.5f;
            item.DeploymentFrequencyBadgeURL = "badge1";
            item.DeploymentFrequencyBadgeWithMetricURL = "badge2";
            item.LeadTimeForChanges = 2.2f;
            item.LeadTimeForChangesBadgeURL = "badge3";
            item.LeadTimeForChangesBadgeWithMetricURL = "badge4";
            item.MeanTimeToRestore = 1.1f;
            item.MeanTimeToRestoreBadgeURL = "badge5";
            item.MeanTimeToRestoreBadgeWithMetricURL = "badge6";
            item.ChangeFailureRate = 0.1f;
            item.ChangeFailureRateBadgeURL = "badge7";
            item.ChangeFailureRateBadgeWithMetricURL = "badge8";
            item.LastUpdatedMessage = "Updated";
            var now = DateTime.UtcNow;
            item.LastUpdated = now;

            // Assert
            Assert.AreEqual("owner", item.Owner);
            Assert.AreEqual("project", item.Project);
            Assert.AreEqual("repo", item.Repo);
            Assert.AreEqual(30, item.NumberOfDays);
            Assert.AreEqual(5.5f, item.DeploymentFrequency);
            Assert.AreEqual("badge1", item.DeploymentFrequencyBadgeURL);
            Assert.AreEqual("badge2", item.DeploymentFrequencyBadgeWithMetricURL);
            Assert.AreEqual(2.2f, item.LeadTimeForChanges);
            Assert.AreEqual("badge3", item.LeadTimeForChangesBadgeURL);
            Assert.AreEqual("badge4", item.LeadTimeForChangesBadgeWithMetricURL);
            Assert.AreEqual(1.1f, item.MeanTimeToRestore);
            Assert.AreEqual("badge5", item.MeanTimeToRestoreBadgeURL);
            Assert.AreEqual("badge6", item.MeanTimeToRestoreBadgeWithMetricURL);
            Assert.AreEqual(0.1f, item.ChangeFailureRate);
            Assert.AreEqual("badge7", item.ChangeFailureRateBadgeURL);
            Assert.AreEqual("badge8", item.ChangeFailureRateBadgeWithMetricURL);
            Assert.AreEqual("Updated", item.LastUpdatedMessage);
            Assert.AreEqual(now, item.LastUpdated);
        }
    }
}
